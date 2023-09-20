"use strict";

class AJAXHelper {
    static getJSONWithoutBlockUI(url: string, onSuccess) {
        $.getJSON(url, onSuccess).fail(jqXHR => this.handleOnFail(jqXHR));
    }

    static post(url: string, formData: any, onSuccess, onComplete?: any) {
        $.blockUI();
        return $.ajax({
            url: url,
            headers: {
                __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            type: "POST",
            data: formData,
            success: function (result) {
                onSuccess(result);
                $.unblockUI();
            },
            error: jqXHR => this.handleOnFail(jqXHR),
            complete: onComplete
        });
    }

    private static handleOnFail(jqXHR) {
        $.unblockUI();
        
        if (jqXHR) {
            if (jqXHR.statusText === "abort") {
                return;
            }

            if (jqXHR.status === 401) {
                window.top["age"].SignOut();
            } else if (jqXHR.status === 500 && jqXHR.responseText) {
                const jsonData = JSON.parse(jqXHR.responseText);
                if (jsonData.UMFrom && jsonData.UMTo) {
                    const cookielessSiteMainUrl = $("#CookielessSiteMainUrl");
                    const siteMainUrl = cookielessSiteMainUrl.length
                        ? cookielessSiteMainUrl.val()
                        : window.top["alpha"].urls.mainRootPath;
                    location.href = `${siteMainUrl}partialum/index?${$.param(jsonData)}`;
                }
            }
        }
    }

    static get(url: string, formData: any, onSuccess, onComplete?: any) {
        $.blockUI();
        return $.ajax({
            url: url,
            headers: {
                __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val(),
                'X-Requested-With': 'XMLHttpRequest'
            },
            type: "GET",
            data: formData,
            success: function (result) {
                onSuccess(result);
                $.unblockUI();
            },
            error: jqXHR => this.handleOnFail(jqXHR),
            complete: onComplete
        });
    }
}