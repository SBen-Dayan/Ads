$(() => {
    $("#eye-button").on('mousedown', () => {
        $("#password").attr('type', 'text');
        $('#eye-icon').attr('class', 'bi bi-eye');
    })

    $('#eye-button').on('mouseup', () => {
        $('#password').attr('type', 'password');
        $('#eye-icon').attr('class', 'bi bi-eye-slash');
    })
})