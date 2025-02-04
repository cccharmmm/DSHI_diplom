document.addEventListener("DOMContentLoaded", function () {
    // Проверяем, обновлялась ли уже страница в этой сессии
    if (!sessionStorage.getItem("pageReloaded")) {
        sessionStorage.setItem("pageReloaded", "true");
        location.reload();
    } else {
        sessionStorage.removeItem("pageReloaded"); // Удаляем флаг после обновления
    }

    function toggleFilter(header, list, arrow) {
        if (list.style.display === "block") {
            list.style.display = "none";
            arrow.style.transform = "rotate(0deg)";
        } else {
            list.style.display = "block";
            arrow.style.transform = "rotate(180deg)";
        }
    }

    function handleFilterClick(header) {
        const list = header.nextElementSibling;
        const arrow = header.querySelector(".arrow");

        document.querySelectorAll(".filter-list").forEach(otherList => {
            if (otherList !== list) {
                otherList.style.display = "none";
                const otherArrow = otherList.previousElementSibling.querySelector(".arrow");
                otherArrow.style.transform = "rotate(0deg)";
            }
        });

        toggleFilter(header, list, arrow);
    }

    document.querySelectorAll(".filter-header").forEach(header => {
        header.addEventListener("click", (event) => {
            event.stopPropagation();
            handleFilterClick(header);
        });
    });

    document.addEventListener("click", (event) => {
        if (!event.target.closest(".filter-header")) {
            document.querySelectorAll(".filter-list").forEach(list => {
                list.style.display = "none";
                const arrow = list.previousElementSibling.querySelector(".arrow");
                arrow.style.transform = "rotate(0deg)";
            });
        }
    });

    document.querySelectorAll('a').forEach(link => {
        link.addEventListener('click', function () {
            sessionStorage.removeItem("pageReloaded"); // Обнуляем флаг перед переходом
            document.querySelectorAll(".filter-list").forEach(list => {
                list.style.display = "none";
                const arrow = list.previousElementSibling.querySelector(".arrow");
                arrow.style.transform = "rotate(0deg)";
            });
        });
    });

    document.querySelectorAll(".filter-list").forEach(list => {
        list.style.display = "none";
        const arrow = list.previousElementSibling.querySelector(".arrow");
        arrow.style.transform = "rotate(0deg)";
    });
});
