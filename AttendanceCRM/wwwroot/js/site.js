// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function getQueryStringByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function errorMessage(message) {
    new RetroNotify({
        style: 'red',
        contentHeader: '',
        contentText: message,
        closeDelay: 5000
    });
}

function successMessage(message) {
    new RetroNotify({
        style: 'green',
        contentHeader: '',
        contentText: message,
        closeDelay: 5000
    });
}

function ShowLoading() {
    $('#preloader').show();
}

function HideLoading() {
    $('#preloader').hide();
}

function InitDeleteConfirmation() {
    $('a.delete').each(function () {
        var $elem = $(this);
        $elem.click(function (e) {
            var confirmMessage = $elem.attr('data-message');
            if (confirmMessage == "" || confirmMessage == undefined) {
                confirmMessage = "Are you sure you want to delete item?";
            }

            var confirmTitle = $elem.attr('data-title');
            if (confirmTitle == "" || confirmTitle == undefined) {
                confirmTitle = "Confirm Delete?";
            }

            $.confirm({
                title: confirmTitle,
                content: confirmMessage,
                type: 'red',
                animation: 'none',
                draggable: false,
                buttons: {
                    primary: {
                        btnClass: 'btn btn-danger',
                        text: "Yes",
                        action: function () {
                            if (typeof DeleteCallback !== 'undefined') {
                                if (DeleteCallback && typeof (DeleteCallback) == typeof (Function)) {
                                    DeleteCallback(e);
                                }
                            }
                        }
                    },
                    cancel: {
                        btnClass: 'btn btn-outline-primary',
                        text: "No",
                        ////action: function () {
                        ////    //$.alert('Deleted the user!');
                        ////}
                    }
                }
            });
        });
    });
}

function PrepareGridBottom(divid) {
    var tblUR = $('#' + divid).find('table').DataTable();
    if (!tblUR.data().count()) {
        $(".grid-bottom").hide();
    }

    $('#' + divid).find('.page-length').find('select').selectpicker();
    ResizeBodyScroll();
}

function SavePageData(obj, formName, successCallBack, errorCallBack) {
    var $form = $('#' + formName);
    $form.data('validator', null);
    $.validator.unobtrusive.parse($form);
    if ($form.valid()) {
        $.ajax({
            type: "POST",
            url: encodeURI($form.attr('action')),
            data: $form.serialize(),
            cache: false,
            async: true,
            dataType: 'json',
            error: function (jqXHR, exception) {
                errorMessage("Something went wrong.");
            },
            success: function (data, textStatus, XMLHttpRequest) {
                if (data.success) {
                    if (successCallBack != null) {
                        successCallBack(data);
                    }
                    else {
                        successMessage(data.message);
                    }
                }
                else {
                    if (errorCallBack != null) {
                        errorCallBack(data);
                    }
                    else {
                        errorMessage(data.message);
                    }
                }
            }
        });
    }

}

function ResizeBodyScroll() {
    if ($('.main-navigation')[0] != undefined) {
        if ($("body")[0].scrollHeight <= window.innerHeight) {
            $("body").css({ height: "100%" });
        }
        else {
            $("body").css({ height: "auto" });
        }
    }
}

function InitiCheck() {
    $('input[type="checkbox"].icheck').iCheck({
        checkboxClass: 'icheckbox_flat-red',
    });
    $('input[type="radio"].icheck').iCheck({
        radioClass: 'iradio_flat-red'
    });
}

function InitSelectPicker() {
    $('.selectpicker').selectpicker({ size: 7, container: 'body' });
}