
//Time Range
$('#reservationtime').daterangepicker({
    timePicker: true,
    timePickerIncrement: 1,

    locale: {
        format: 'hh:mm A'
    }
}).on('show.daterangepicker', function (ev, picker) {
    picker.container.find(".calendar-table").hide(); // hide the calendar table
    picker.container.find(".ranges").hide(); // hide the preset ranges dropdown
    picker.container.find(".timepicker").show(); // show only the timepicker
});

//date Range
$('#reservationdate').daterangepicker({
    locale: {
        format: 'MM/DD/YYYY'
    }
})