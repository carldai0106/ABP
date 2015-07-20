$(function () {
    
    var lang = $.parseJSON($("#LocalizationLang").html());

    $("a.switcher").on("click", function () {
        if ($(window).width() >= 534) {
            if ($(this).attr("data") === "true") {
                setTimeout(function() {
                    $("aside.nav-aside").removeClass("mini-nav-aside");
                    $("#main-container").animate({ "left": "220px" }, 400);
                    $(".nav-aside").animate({ width: "220px" }, 400, function() {
                        $("ul.carte > li > a > span").show(200);
                        $(".nav-wrapper li.active .carte-children").slideDown(200);
                        $(".nav-wrapper").removeClass('mini');
                        $("a.switcher").attr("data", "false");
                        $("ul.carte").css({ overflow: "hidden" });
                        $(".slimScrollBar").css({ width: "7px" });
                    });
                }, 600);
                //$.post("/Config/Set/", { key: "AutoCloseMenu", value: "false" });
            } else {
                $("ul.carte > li > a > span").hide(200);
                var selector = ".nav-wrapper li.active .carte-children";
                if ($(".nav-wrapper li.active").size() === 0) {
                    selector = ".nav-wrapper li .carte-children";
                }
                $(selector).slideUp(200, function() {
                    $(".nav-aside").animate({ width: "50px" }, 400, function() {
                        $(this).addClass("mini-nav-aside");
                    });
                    $("#main-container").animate({ left: "50px" }, 400);
                    $(".nav-wrapper").addClass('mini');
                    $("a.switcher").attr("data", "true");
                    $("ul.carte").css({ overflow: "" });
                    $(".slimScrollBar").css({ width: "0" });
                });
                //$.post("/Config/Set/", { key: "AutoCloseMenu", value: "true" });
            }
        }
        return false;
    });

    $(window).on("resize", function() {
        var width = $(this).width();
        var data = $("a.switcher").attr("data");
        //console.log(width + " " + data);
        if (width < 534) {
            $("#main-container").css({ left: "0" });
        } else if (width > 534 && data === "true") {
            $("#main-container").css({ left: "50px" });
        } else if (data === "false" || data == undefined) {
            $("#main-container").css({ left: "220px" });
        }
    });

    $('.carte').slimScroll({
        height: '100%'
    });

    $(".carte li>a").on("click", function () {
        var value = $(this).attr("href");
        if (value === "#") {
            var parent = $(this).parent();

            parent.siblings("li").children(".carte-children").slideUp(300, function () {
                $(this).parent("li").removeClass("active");
            });
            parent.siblings("li").find("a>i").removeClass("icon-spread");

            parent.siblings("li").find("li").children(".carte-children").slideUp(300, function () {
                $(this).parent("li").removeClass("active");
            });
            parent.siblings("li").find("li>a>i").removeClass("icon-spread");

            parent.children(".carte-children").slideToggle(300, function () {
                $(this).parent("li").addClass("active");
            });

            var isMenuChildren = parent.parent().is(".carte-children");
            var hasMenuChildren = $(this).next().is(".carte-children");
            var size = $(this).children("i.icon-spread").size();
            if (hasMenuChildren && isMenuChildren && size == 0) {
                $(this).children("i").addClass("icon-spread");
            } else {
                $(this).children("i.icon-spread").removeClass("icon-spread");
            }
        }
    });

    $('.fancybox').fancybox({
        padding: 0,
        openEffect: "fade",
        closeEffect: "elastic",
        afterLoad: function (current, previous) {
            if ($(current["content"]).find(".errorwrapper").size() > 0) {
                $("body").html(current["content"]);
            }
            else {
                var url = $(current["content"]).find('form').attr('action');
                if (url != undefined && url.indexOf("login")) {
                    location.href = window.location.pathname;
                }
            }
        },
        helpers: {
            overlay: {
                closeClick: false, 
            },
            title: null
        }
    });

    $("body").on("click", ".fancybox-inner a.btn.cancel", function () {
        $.fancybox.close();
        return false;
    });

    $(".powertip").powerTip({
        placement: 's',
        smartPlacement: true
    });

    $(".table th input:checkbox").on("click", function () {
        var that = this;
        $(this).closest('table').find('tr > td:first-child input:checkbox').each(function () {
            this.checked = that.checked;
            if (that.checked) {
                $(this).closest('tr').addClass('active');
            } else {
                $(this).closest('tr').removeClass('active');
            }
        });
    });

    //µã»÷ÐÐ
    $(".table td").not(":first-child").not(":not(.last):last-child").on("click", function () {
        $(this).closest('tr').toggleClass('active');
        var checkbox = $(this).closest('tr').find("input:checkbox").get(0);
        if (checkbox.checked) {
            checkbox.checked = false;
        } else {
            checkbox.checked = true;
        }
    });


    $(".table input:checkbox").on("change", function () {
        if (this.checked) {
            $(this).closest('tr').addClass('active');
        } else {
            $(this).closest('tr').removeClass('active');
        }
    });

    $(".table tbody tr").on({
        mouseenter: function () {
            $(this).addClass("hover");
        }, mouseleave: function () {
            $(this).removeClass("hover");
        }
    });

    $(".btn.edit, .nav-btn.edit").on("click", function () {
        var list = [];
        $(".table").find('tr > td:first-child input:checkbox').each(function () {
            if (this.checked) {
                list.push($(this).val());
            }
        });
        if (list.length === 0) {
            openAlert(lang.SelectToEdit);
            return false;
        }
        else if (list.length > 1) {
            openAlert(lang.SelectOneToEdit);
            return false;
        } else {
            var id = list[0];
            var url = $(this).attr("rel");
            $(this).attr("href", url + id);
        }
    });

    $(".btn.delete, .nav-btn.delete").on("click", function () {
        var arr = [];
        $(".table").find('tr > td:first-child input:checkbox').each(function () {
            if (this.checked) {
                arr.push($(this).val());
            }
        });
        if (arr.length === 0) {
            openAlert(lang.SelectToDelete);
            return false;
        } else {
            var url = $(this).attr("rel");
            openConfirm(lang.SureDelete, function () {
                var toUrl = "";
                if (url.indexOf("?") >= 0) {
                    toUrl = url + "&id=" + arr.join(",");
                } else {
                    toUrl = url + arr.join(",");
                }
                window.location = toUrl;
            });

            return false;
        }
    });

    if ($(".message-box").length > 0) {
        $(".message-box").slideDown("normal", "linear", function () {
            setTimeout(function () {
                $(".message-box").slideUp("normal");
            }, 5000);
        });
    }
});

function openLoading() {
    $("#dlgload").dialog({
        position: {
            my: "center top",
            at: "center top+100px",
        },
        modal: true,
        dialogClass: "ui-dialog-front",
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        }
    });
}

function closeLoading() {
    $("#dlgload").dialog("close");
}

function openAlert(message) {

    var lang = $.parseJSON($("#LocalizationLang").html());

    $("#dlgmsg").attr("title", lang.Information);
    $("#dlgmsg-text").html(message);
    $("#dlgmsg").dialog({
        position: {
            my: "center top",
            at: "center top+200px",
        },
        width: 400,
        modal: true,
        dialogClass: "ui-dialog-front",
        buttons: {
            Ok: function () {
                $(this).dialog("close");
            }
        }
    });
}

function openConfirm(message, callback, title) {
    var lang = $.parseJSON($("#LocalizationLang").html());

    if (typeof (title) === "undefined" || title === "") {
        title = lang.Confirmation;
    }

    $("#dlgmsg").attr("title", title);
    $("#dlgmsg-text").html(message);
    $("#dlgmsg").dialog({
        position: {
            my: "center top",
            at: "center top+200px",
        },
        width: 400,
        modal: true,
        dialogClass: "ui-dialog-front",
        buttons: {
            Ok: function () {
                $(this).dialog("close");
                if ($.isFunction(callback)) {
                    callback.apply();
                }
            },
            Cancel: function () {
                $(this).dialog("close");
            }
        }
    });
}

