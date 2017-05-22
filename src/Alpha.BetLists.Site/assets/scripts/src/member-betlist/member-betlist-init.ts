$(function () {
    const memberBetListView = new MemberBetListView(
        $,
        new DateRangePickerComponent($),
        new ProductMultiSelectionComponent($),
        new SingleSelectionDropdownComponent($),
        window.location);

    memberBetListView.init();
})