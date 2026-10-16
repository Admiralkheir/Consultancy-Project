// Appointment calendar (FullCalendar 6). Talks to HomeOfficeController: GetEvents, SaveEvent, DeleteEvent.
(function () {
    'use strict';

    var el = document.getElementById('calendar');
    if (!el) return;

    var urls = { events: el.dataset.eventsUrl, save: el.dataset.saveUrl, del: el.dataset.deleteUrl };
    var canBook = el.dataset.canBook === 'true';
    var token = document.querySelector('meta[name="csrf-token"]').content;
    var filter = document.getElementById('expertFilter');
    var modalEl = document.getElementById('appointmentModal');
    var modal = new bootstrap.Modal(modalEl);
    var form = document.getElementById('appointmentForm');
    var f = {
        id: document.getElementById('apptId'),
        expert: document.getElementById('apptExpert'),
        client: document.getElementById('apptClient'),
        subject: document.getElementById('apptSubject'),
        date: document.getElementById('apptDate'),
        start: document.getElementById('apptStart'),
        end: document.getElementById('apptEnd'),
        allDay: document.getElementById('apptAllDay'),
        description: document.getElementById('apptDescription'),
        error: document.getElementById('apptError'),
        del: document.getElementById('apptDelete'),
        title: document.getElementById('appointmentModalTitle')
    };

    // ---- helpers

    function pad(n) { return (n < 10 ? '0' : '') + n; }
    function dateStr(d) { return d.getFullYear() + '-' + pad(d.getMonth() + 1) + '-' + pad(d.getDate()); }
    function timeStr(d) { return pad(d.getHours()) + ':' + pad(d.getMinutes()); }
    // Appointments are stored as wall-clock times, so they are sent without a UTC offset.
    function iso(d) { return dateStr(d) + 'T' + timeStr(d) + ':00'; }
    function addMinutes(d, m) { return new Date(d.getTime() + m * 60000); }

    function toast(message, ok) {
        var container = document.querySelector('.toast-container');
        if (!container) {
            container = document.createElement('div');
            container.className = 'toast-container position-fixed bottom-0 end-0 p-3';
            document.body.appendChild(container);
        }
        var t = document.createElement('div');
        t.className = 'toast align-items-center border-0 ' + (ok ? 'text-bg-success' : 'text-bg-danger');
        t.setAttribute('role', ok ? 'status' : 'alert');
        t.innerHTML = '<div class="d-flex"><div class="toast-body"></div><button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Kapat"></button></div>';
        t.querySelector('.toast-body').textContent = message;
        container.appendChild(t);
        t.addEventListener('hidden.bs.toast', function () { t.remove(); });
        new bootstrap.Toast(t, { delay: 5000 }).show();
    }

    function post(url, data) {
        return fetch(url, {
            method: 'POST',
            headers: { 'Content-Type': 'application/x-www-form-urlencoded', 'RequestVerificationToken': token },
            body: new URLSearchParams(data)
        }).then(function (r) {
            if (!r.ok) throw new Error('HTTP ' + r.status);
            return r.json();
        });
    }

    function payloadFromEvent(event) {
        var p = event.extendedProps;
        var end = event.allDay ? null : (event.end || addMinutes(event.start, 60));
        return {
            AppointmentID: event.id,
            ExpertID: p.expertId,
            ClientID: p.clientId,
            Subject: event.title,
            Description: p.description || '',
            ThemeColor: event.backgroundColor,
            IsFullDay: event.allDay,
            StartTime: iso(event.start),
            EndTime: end ? iso(end) : ''
        };
    }

    // ---- form

    function setAllDay(on) {
        f.allDay.checked = on;
        form.querySelectorAll('.time-field').forEach(function (x) { x.classList.toggle('d-none', on); });
    }
    f.allDay.addEventListener('change', function () { setAllDay(f.allDay.checked); });

    function openForm(a) {
        f.error.classList.add('d-none');
        f.id.value = a.id || 0;
        f.title.textContent = a.id ? 'Randevuyu düzenle' : 'Yeni randevu';
        f.del.classList.toggle('d-none', !a.id);
        f.del.dataset.armed = '';
        f.del.innerHTML = '<i class="bi bi-trash me-1"></i>Sil';
        if (a.expertId) { f.expert.value = a.expertId; }
        else if (filter.value) { f.expert.value = filter.value; }
        else { f.expert.selectedIndex = 0; }
        if (a.clientId) { f.client.value = a.clientId; } else { f.client.selectedIndex = 0; }
        f.subject.value = a.title || '';
        f.description.value = a.description || '';
        f.date.value = dateStr(a.start);
        f.start.value = a.allDay ? '09:00' : timeStr(a.start);
        f.end.value = a.allDay ? '10:00' : timeStr(a.end || addMinutes(a.start, 60));
        setAllDay(!!a.allDay);
        var color = form.querySelector('input[name="apptColor"][value="' + (a.color || '').toLowerCase() + '"]')
            || form.querySelector('input[name="apptColor"]:checked')
            || form.querySelector('input[name="apptColor"]');
        color.checked = true;
        modal.show();
    }
    modalEl.addEventListener('shown.bs.modal', function () { f.subject.focus(); });

    function showError(message) {
        f.error.textContent = message;
        f.error.classList.remove('d-none');
    }

    form.addEventListener('submit', function (e) {
        e.preventDefault();
        var allDay = f.allDay.checked;
        if (!f.subject.value.trim()) return showError('Konu boş olamaz.');
        if (!f.date.value) return showError('Tarih seçiniz.');
        if (!allDay && (!f.start.value || !f.end.value)) return showError('Başlangıç ve bitiş saatini giriniz.');
        if (!allDay && f.end.value <= f.start.value) return showError('Bitiş saati başlangıçtan sonra olmalıdır.');

        var data = {
            AppointmentID: f.id.value,
            ExpertID: f.expert.value,
            ClientID: f.client.value,
            Subject: f.subject.value.trim(),
            Description: f.description.value,
            ThemeColor: form.querySelector('input[name="apptColor"]:checked').value,
            IsFullDay: allDay,
            StartTime: f.date.value + 'T' + (allDay ? '00:00' : f.start.value) + ':00',
            EndTime: allDay ? '' : f.date.value + 'T' + f.end.value + ':00'
        };
        var button = document.getElementById('apptSave');
        button.disabled = true;
        post(urls.save, data).then(function (res) {
            if (!res.status) return showError(res.message || 'Randevu kaydedilemedi.');
            modal.hide();
            calendar.refetchEvents();
            toast(f.id.value !== '0' ? 'Randevu güncellendi.' : 'Randevu oluşturuldu.', true);
        }).catch(function () {
            showError('Sunucuya ulaşılamadı. Lütfen tekrar deneyin.');
        }).finally(function () { button.disabled = false; });
    });

    // Two-step delete inside the modal: the first click arms the button.
    f.del.addEventListener('click', function () {
        if (!f.del.dataset.armed) {
            f.del.dataset.armed = 'true';
            f.del.innerHTML = '<i class="bi bi-exclamation-triangle me-1"></i>Silmeyi onayla';
            return;
        }
        post(urls.del, { id: f.id.value }).then(function (res) {
            if (!res.status) return showError('Randevu silinemedi.');
            modal.hide();
            calendar.refetchEvents();
            toast('Randevu silindi.', true);
        }).catch(function () { showError('Sunucuya ulaşılamadı.'); });
    });

    // ---- calendar

    function moved(info) {
        post(urls.save, payloadFromEvent(info.event)).then(function (res) {
            if (!res.status) {
                info.revert();
                toast(res.message || 'Randevu taşınamadı.', false);
            } else {
                toast('Randevu taşındı.', true);
            }
        }).catch(function () {
            info.revert();
            toast('Sunucuya ulaşılamadı.', false);
        });
    }

    var compact = window.matchMedia('(max-width: 767.98px)').matches;
    var calendar = new FullCalendar.Calendar(el, {
        locale: 'tr',
        initialView: compact ? 'listWeek' : 'timeGridWeek',
        headerToolbar: compact
            ? { left: 'prev,next', center: 'title', right: 'today' }
            : { left: 'prev,next today', center: 'title', right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek' },
        footerToolbar: compact ? { center: 'listWeek,timeGridDay,dayGridMonth' } : false,
        firstDay: 1,
        nowIndicator: true,
        slotMinTime: '07:00:00',
        slotMaxTime: '22:00:00',
        scrollTime: '08:30:00',
        slotLabelFormat: { hour: '2-digit', minute: '2-digit', hour12: false },
        eventTimeFormat: { hour: '2-digit', minute: '2-digit', hour12: false },
        businessHours: { daysOfWeek: [1, 2, 3, 4, 5, 6], startTime: '09:00', endTime: '19:00' },
        defaultTimedEventDuration: '01:00:00',
        contentHeight: compact ? 'auto' : 680,
        expandRows: true,
        dayMaxEvents: true,
        eventDisplay: 'block',
        selectable: canBook,
        selectMirror: true,
        editable: true,
        events: function (info, success, failure) {
            var params = new URLSearchParams({ start: info.startStr, end: info.endStr });
            if (filter.value) params.set('expertId', filter.value);
            fetch(urls.events + '?' + params.toString(), { headers: { 'Accept': 'application/json' } })
                .then(function (r) { if (!r.ok) throw new Error('HTTP ' + r.status); return r.json(); })
                .then(function (rows) {
                    success(rows.map(function (v) {
                        var color = v.ThemeColor || '#2563eb';
                        return {
                            id: String(v.AppointmentID),
                            title: v.Subject,
                            start: v.StartTime,
                            end: v.EndTime,
                            allDay: v.IsFullDay,
                            backgroundColor: color,
                            borderColor: color,
                            extendedProps: {
                                expertId: v.ExpertID, clientId: v.ClientID, description: v.Description,
                                expertName: v.ExpertName, clientName: v.ClientName
                            }
                        };
                    }));
                })
                .catch(function (err) {
                    toast('Randevular yüklenemedi.', false);
                    failure(err);
                });
        },
        eventContent: function (arg) {
            var p = arg.event.extendedProps;
            // In the list view the text sits in a table cell: truncating there would widen the table instead of wrapping.
            var truncate = arg.view.type.indexOf('list') === 0 ? '' : ' text-truncate';
            var title = document.createElement('div');
            title.className = 'fw-semibold' + truncate;
            title.textContent = arg.event.title;
            var meta = document.createElement('div');
            meta.className = 'small' + truncate;
            meta.style.opacity = '.85';
            meta.textContent = [arg.timeText, p.clientName, filter.value ? null : p.expertName].filter(Boolean).join(' · ');
            return { domNodes: [title, meta] };
        },
        eventDidMount: function (info) {
            var p = info.event.extendedProps;
            info.el.title = info.event.title + '\nUzman: ' + p.expertName + (p.clientName ? '\nDanışan: ' + p.clientName : '') + (p.description ? '\n' + p.description : '');
        },
        select: function (info) {
            calendar.unselect();
            openForm({ start: info.start, end: info.end, allDay: info.allDay });
        },
        eventClick: function (info) {
            var e = info.event, p = e.extendedProps;
            openForm({
                id: e.id, title: e.title, start: e.start, end: e.end, allDay: e.allDay,
                expertId: p.expertId, clientId: p.clientId, description: p.description, color: e.backgroundColor
            });
        },
        eventDrop: moved,
        eventResize: moved
    });
    calendar.render();

    filter.addEventListener('change', function () { calendar.refetchEvents(); });

    var newButton = document.getElementById('newAppointment');
    newButton.addEventListener('click', function () {
        var start = new Date();
        start.setMinutes(start.getMinutes() < 30 ? 30 : 60, 0, 0);
        openForm({ start: start, end: addMinutes(start, 60), allDay: false });
    });
})();
