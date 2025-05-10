
var deleteImageFromProductBtns = document.querySelectorAll(".delete-btn");

deleteImageFromProductBtns.forEach((btn) => {
    btn.addEventListener("click", function () {

        let imageId =parseInt(this.parentNode.getAttribute("data-id"));
        fetch('http://localhost:62859/Admin/Product/DeleteImageFromProduct?id=' + imageId, {
            method: "POST",
            headers: {
                "Content-type": "application/json; charset=UTF-8"
            }

        }).then(response => response.text()).then(res => {
            this.parentNode.parentNode.remove();
        });

    })
});

var setImageMainBtns = document.querySelectorAll(".set-btn");

setImageMainBtns.forEach((btn) => {
    btn.addEventListener("click", function () {

        let imageId = parseInt(this.parentNode.getAttribute("data-id"));
        fetch('http://localhost:62859/Admin/Product/SetImageMain?id=' + imageId, {
            method: "POST",
            headers: {
                "Content-type": "application/json; charset=UTF-8"
            }

        }).then(response => response.text()).then(res => {
            document.querySelector(".border-main").classList.remove("border-main");
            this.parentNode.parentNode.classList.add("border-main");
        });

    })
});
