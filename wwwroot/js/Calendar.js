$(document).ready(function () {
    var events = [];
    
    $.ajax({
        type: "GET",
        url: "/home/GetEvents",
        contentType: "application/json",
        dataType:"json",
        success: function (data) {
            
            var newData = data.data;
            $.each(newData, function (i,v) {                
                events.push({   
                    title: v.subject,
                    description: v.description,
                    start: moment(v.start),
                    end: v.stop != null ? moment(v.stop) : null,
                    color: v.themeColor,
                    allDay: v.isFullDay
                });
                
            })            
            GenerateCalendar(events);
        },
        error: function (error) {
            alert('failed');
        }

    })

    function GenerateCalendar(events) {
        $('#calendar').fullCalendar('destroy');

        $('#calendar').fullCalendar({
            contentHeight: 600,            
            defaultDate: new Date(),
            timeFormat: 'h(:mm)a',
            header: {
                left: 'prev,next today',
                center: 'title',
                right: 'month,basicWeek,basicDay,agenda'
            },
            eventLimit: true,
            eventColor: '#378006',
            events: events,
            eventClick: function (calEvent, jsEvent, view) {
                $('#myModal #eventTitle').text(calEvent.title);
                var $description = $('<div/>');
                $description.append($('<p/>').html('<b>Start:</b>' + calEvent.start.format("DD-MMM-YYYY HH:mm a")));
                if (calEvent.end != null) {
                    $description.append($('<p/>').html('<b>End:</b>' + calEvent.end.format("DD-MMM-YYYY HH:mm a")));
                }
                $description.append($('<p/>').html('<b>Description:</b>' + calEvent.description));
                $('#myModal #pDetails').empty().html($description);

                $('#myModal').modal();
            }

        })
    }

})