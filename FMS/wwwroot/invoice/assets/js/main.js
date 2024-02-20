(function ($) {
    "use strict";

    $("#download_btn").on("click", function () {
        var $downloadSection = $("#download_section"),
            sectionWidth = $downloadSection.width(),
            sectionHeight = $downloadSection.height(),
            pageWidth = sectionWidth + 80,
            pageHeight = 1.5 * pageWidth + 80,
            totalPages = Math.ceil(sectionHeight / pageHeight),
            currentPage = 1;

        html2canvas($downloadSection[0], { allowTaint: true }).then(function (canvas) {
            var context = canvas.getContext("2d"),
                imageData = canvas.toDataURL("image/jpeg", 1),
                pdf = new jsPDF("p", "pt", [pageWidth, pageHeight]);

            addImageAndPageNumbers(0);

            function addImageAndPageNumbers(offset) {
                // Add page number
                pdf.text(580, pageHeight - 20, "Page " + currentPage + " of " + totalPages);

                // Add the image
                pdf.addImage(imageData, "JPG", 40, 40 + offset, sectionWidth, sectionHeight);

                // If there are more pages, add a new page and repeat
                if (currentPage < totalPages) {
                    pdf.addPage();
                    currentPage++;
                    addImageAndPageNumbers(offset + pageHeight);
                } else {
                    pdf.save("th-invoice.pdf");
                }
            }
        });
    });

    $(".print_btn").on("click", function () {
        window.print();
    });

    // Background image handling
    $("[data-bg-src]").each(function () {
        var $element = $(this),
            bgSrc = $element.attr("data-bg-src");

        $element.css("background-image", "url(" + bgSrc + ")")
            .removeAttr("data-bg-src")
            .addClass("background-image");
    });

    // Preventing context menu and keyboard shortcuts
    window.addEventListener("contextmenu", function (event) {
        event.preventDefault();
    }, false);

    document.onkeydown = function (event) {
        return !(event.keyCode == 123 || (event.ctrlKey && event.shiftKey && event.keyCode == "I".charCodeAt(0)) || (event.ctrlKey && event.shiftKey && event.keyCode == "C".charCodeAt(0)) || (event.ctrlKey && event.shiftKey && event.keyCode == "J".charCodeAt(0)) || (event.ctrlKey && event.keyCode == "U".charCodeAt(0)));
    };
})(jQuery);
