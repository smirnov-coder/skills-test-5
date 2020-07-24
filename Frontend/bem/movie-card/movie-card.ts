import $ from "jquery";

$(document).ready(() => {
    $(".movie-card").hover(
        function () {
            $(this).addClass("shadow");
        },
        function () {
            $(this).removeClass("shadow");
        }
    );
});
