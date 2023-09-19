"use strict";
declare var Clipboard: any;

class MonitoringBetsCopyHtml {
    init() {
        $("#icon-copy").tooltip();
        const clipboard = new Clipboard("#icon-copy", {
            text: () => this._copyTableHtml($("#tbl-betlist"))
        });

        if (window.document.queryCommandSupported && window.document.queryCommandSupported("copy")) {
            clipboard.on("success", function (e) {
                const $this = $(e.trigger);
                $this.attr("data-original-title", $("#hidden-CopiedText").val());
                $this.tooltip("show");
                setTimeout(() => {
                    $this.tooltip("hide");
                    $this.attr("data-original-title", $("#hidden-CopyText").val());
                }, 3000);
            }).on("error", function (e) {
                $(e.trigger).remove();
                console.error("Action:", e.action);
                console.error("Trigger:", e.trigger);
            });
        }
    }

    _copyTableHtml(tableReport: JQuery) {
        const html = ['<table>'];
        this._appendTableHeader(tableReport, html);
        this._appendTableBody(tableReport, html);
        html.push('</table>');

        return html.join("");
    }

    _appendTableHeader(tableReport: JQuery, html) {
        html.push('<thead><tr>');
        tableReport.find("thead tr:first th").each(function () {
            const $this = $(this);
            html.push('<th>', this.innerText, '</th>');
        });

        html.push('</tr></thead>');
    }

    _appendTableBody(tableReport: JQuery, html) {
        html.push('<tbody>');

        tableReport.find("tbody tr").each(function () {
            html.push('<tr>');
            $(this).find("td").each(function (index) {
                html.push('<td>', index === 1 ? "'" : "", this.innerText, '</td>');
            });
            html.push('</tr>');
        });

        html.push('</tbody>');
    }
}