import $ from "jquery";

$(document).ready(() => {
    $("#page-size-selector").on("change", function () {
        const pageSize = (this as HTMLSelectElement).value;
        const link = document.createElement("a");
        const urlParams = new URLSearchParams(window.location.search);
        const currentPage = urlParams.get('page');
        const page = currentPage || "1";
        link.href = `/?page=${page}&pageSize=${pageSize}`;
        link.click();
    });
});