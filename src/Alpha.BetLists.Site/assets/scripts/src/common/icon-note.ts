class IconInformation {
    private $: JQueryStatic;
    constructor($: JQueryStatic) {
        this.$ = $;
    }

    init = () => {
        this.registerEvents();
    }

    registerEvents = () => {
        var self = this;

        this.$("#icon-information").on("click", this.showGuidelines);
    }

    showGuidelines = () => {
        const iconInformation = this.$("#icon-information");
        const guideNote = this.$("#info-note");
        guideNote.modal({ show: true, backdrop: false });

        const iconHelpPosition = iconInformation.position();
        guideNote.css({
            bottom: "auto",
            top: iconHelpPosition.top + 40,
        });
    };

}    

$(() => {
    new IconInformation($).init();    
});

