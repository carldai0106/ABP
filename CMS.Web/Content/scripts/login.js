$(function () {

    var timezoneOffset = new Date().getTimezoneOffset();
    $("#TimezoneOffset").val(timezoneOffset);

    function setPosition() {
        var top = ($(window).height() - $(".signin-block").height() - 100) / 2;
        if (top > 0)
            $(".signin-block").css("margin-top", top);
    }

    setPosition();

    $(window).on("resize", function () {
        setPosition();
    });

    $("#btnLogin").on("click", function () {
        $("form").submit();
    });
    if ($(".message-box").length > 0) {
        $(".message-box").slideDown("normal", "linear", function () {
            setTimeout(function () {
                $(".message-box").slideUp("normal");
            }, 5000);
        });
    }
});