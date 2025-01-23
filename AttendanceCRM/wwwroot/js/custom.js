$(document).ready(function () {
    var $nav = $(".main-navigation");
    var $speed = 300;
    var $onlyArrowClick = "false";
    var $mobileMenu = 991;
    var $scrollBg = "rgba(0, 0, 0, 0)";
    var $scrollColor = "rgba(255, 255, 255, 0.47)";
    var $scrollWidth = 5;

    if ($nav.find('.open').length) {
        $nav.find('.open').children('ul').show();
    }

    $nav.find("ul").each(function () {
        $(this).siblings("a").append("<i class='arrow-icon'></i>").closest('li').addClass('has-menu');
    });

    $nav.find("a").click(function (e) {
        if ($onlyArrowClick === "false" && $(this).find(".arrow-icon").length > 0) {
            e.preventDefault();
            $(this).find(".arrow-icon").click();
        }
    });

    $nav.find("ul li a .arrow-icon").click(function (e) {
        e.preventDefault();
        e.stopImmediatePropagation();
        var $this = $(this).closest('li');
        if (!$this.hasClass('open')) {
            $this.siblings('.open').find("ul").stop(true, true).slideUp($speed);
            $this.siblings('.open').removeClass('open').find(".open").removeClass('open');
            $this.addClass('open').find("> ul").slideDown($speed, function () {
                resizeScroll();
            });
        } else {
            $this.find("ul").stop(true, true).slideUp($speed, function () {
                resizeScroll();
            });
            $this.removeClass('open').find(".open").removeClass('open');
        }
    });



    $(".nav-icon, .menu-overlay").click(function () {
        if ($(window).outerWidth() >= $mobileMenu)
            $("body").toggleClass("menu-open");
        else
            $("body").toggleClass("menu-open-mobile");
        return false;
    });


    // Custom Scroll
    function addScroll() {
        $nav.niceScroll($nav.find("> ul"), {
            cursorcolor: $scrollColor,
            background: $scrollBg,
            cursorwidth: $scrollWidth,
            horizrailenabled: false,
            cursorborder: "none",
            cursorborderradius: 0,
            autohidemode: false,
            bouncescroll: false
        });
    }
    function resizeScroll() {
        $nav.getNiceScroll().resize();
    }
    addScroll();

    //Prevent Page Reload on all # links
    $("a[href='#']").click(function (e) {
        e.preventDefault();
    });

    //placeholder
    $("[placeholder]").each(function () {
        $(this).attr("data-placeholder", this.placeholder);

        $(this).bind("focus", function () {
            this.placeholder = '';
        });
        $(this).bind("blur", function () {
            this.placeholder = $(this).attr("data-placeholder");
        });
    });

    // uploadBtn
    $('.custom-file-input').on('change', function () {
        //get the file name
        var fileName = $(this).val();
        //replace the "Choose a file" label
        $(this).next('.custom-file-label').html(fileName);
    })

    //datepicker
    $('.datepicker').datepicker({
        autoclose: true,
        format: "dd/M/yyyy",
        ignoreReadonly: true
    });

    $('.dropdown-toggle').dropdown({
        container: 'body'

    });


    //introJs(".introduction-farm").start();

    // Aside-panel-open ================================================
    $(".filter-panel .panel-body").niceScroll({
        cursorcolor: "#c5c5c5",
        background: false,
        cursorwidth: 5,
        cursorborder: "none",
        cursorborderradius: 0,
        autohidemode: false,
        bouncescroll: false,
    });


    // Add remove class when window resize finished
    var $resizeTimer;
    $(window).on("resize", function (e) {
        if (!$("body").hasClass("window-resizing"))
            $("body").addClass("window-resizing");
        clearTimeout($resizeTimer);
        $resizeTimer = setTimeout(function () {
            $("body").removeClass("window-resizing");
        }, 250);
    });

    // Check your browser is supported webp or not
    function WebpIsSupported(callback) {
        if (!window.createImageBitmap) {
            callback(false);
            return;
        };
        var webpdata = 'data:image/webp;base64,UklGRiQAAABXRUJQVlA4IBgAAAAwAQCdASoCAAEAAQAcJaQAA3AA/v3AgAA=';
        fetch(webpdata).then(function (response) {
            return response.blob();
        }).then(function (blob) {
            createImageBitmap(blob).then(function () {
                callback(true);
            }, function () {
                callback(false);
            });
        });
    }

    // Replace .webp to .jpg or .png
    WebpIsSupported(function (isSupported) {
        if (!isSupported) {
            $("img").each(function (e) {
                var $this = $(this);
                var $exe = "jpg";
                var $src = $this.attr("src");
                if ($this.attr("data-type") != undefined)
                    $exe = $this.attr("data-type");

                if ($src.indexOf(".webp") > -1) {
                    $src = $src.replace(".webp", "." + $exe);
                }
                $(this).attr('src', $src);
            });
        };
    });

    // Add Class on Window Load
    $("body").addClass("page-loaded");
})