
let relatedCategories = document.querySelector(".related-products-categories");
if (relatedCategories && relatedCategories.firstElementChild) {
    relatedCategories.firstElementChild.classList.add("active");
}


let tabContent = document.querySelector(".tab-content");
if (tabContent && tabContent.firstElementChild) {
    tabContent.firstElementChild.classList.add("active");
}
//add product to basket

let addBasketBtn = document.querySelectorAll(".add-basket");

addBasketBtn.forEach((btn) => {
    btn.addEventListener("click", function () {

        let productId = this.getAttribute("data-id");
        fetch('http://localhost:62859/Home/AddProductToBasket?id=' + productId, {
            method: "POST",
            headers: {
                "Content-type": "application/json; charset=UTF-8"
            }

        }).then(response => response.json()).then(res => {
            document.querySelector(".basket-count-show").innerText = res.basketCount;
            document.querySelector(".sub-total span").innerText = res.total;
        });

    })
});


let deleteFromBasketBtns = document.querySelectorAll(".delete-basket");
 
deleteFromBasketBtns.forEach((btn) => {
    btn.addEventListener("click", function () {
        event.preventDefault();
        let productId = this.parentNode.getAttribute("data-id");
        fetch('http://localhost:62859/Cart/Delete?id=' + productId, {
            method: "POST",
            headers: {
                "Content-type": "application/json; charset=UTF-8"
            }

        }).then(response => response.json()).then(res => {
            this.parentNode.parentNode.parentNode.remove();
            document.querySelector(".basket-count-show").innerText = res.basketCount;
            document.querySelector(".sub-total span").innerText = res.total;
            if (res.basketCount == 0) {
                document.querySelector(".cart-empty-alert").classList.remove("d-none");
                document.querySelector(".shopping-cart-container").classList.add("d-none");
            }
        });

    })
})

let deleteAllFromBasket = document.querySelector(".delete-all-basket");

deleteAllFromBasket.addEventListener("click", function () {
    fetch('http://localhost:62859/Cart/DeleteAll', {
        method: "POST",
        headers: {
            "Content-type": "application/json; charset=UTF-8"
        }

    }).then(response => response.json()).then(res => {
        //this.parentNode.parentNode.parentNode.remove();
        //document.querySelector(".basket-count-show").innerText = res.basketCount;
        //document.querySelector(".sub-total span").innerText = res.total;
        //if (res.basketCount == 0) {
        //    document.querySelector(".cart-empty-alert").classList.remove("d-none");
        //    document.querySelector(".shopping-cart-container").classList.add("d-none");
        //}
    });

})