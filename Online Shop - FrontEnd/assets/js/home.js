"use strict";
$(function () {
  $("#product-slider").owlCarousel({
    items: 4,
    itemsDestop: [1199, 2],
    itemsDestopSmall: [980, 2],
    itemsMoile: [700, 1],
    pagination: false,
    navigation: true,
    navigationText: ["", ""],
    autoPlay: true,
  });

  $("#product-slider-2").owlCarousel({
    items: 4,
    itemsDestop: [1199, 2],
    itemsDestopSmall: [980, 2],
    itemsMoile: [700, 1],
    pagination: false,
    navigation: true,
    navigationText: ["", ""],
    autoPlay: true,
  });

  let scrollSection = document.getElementById("scroll-section");

  window.onscroll = function () {
    scrollFunction();
  };

  function scrollFunction() {
    if (
      document.body.scrollTop > 195 ||
      document.documentElement.scrollTop > 195
    ) {
      scrollSection.style.top = "0";
    } else {
      scrollSection.style.top = "-200px";
      $("div").removeClass("inActive");
    }
  }

  let addHeart = document.querySelectorAll("#products .product .fa-heart");
  let heartCount = document.querySelector(".heart sup");
  let heartCount2 = document.querySelector("#scroll-section .heart sup");

  let products = [];

  if (localStorage.getItem("products") != null) {
    products = JSON.parse(localStorage.getItem("products"));
  }

  addHeart.forEach((heartBtn) => {
    heartBtn.addEventListener("click", function (e) {
      heartBtn.classList.toggle("my-active");

      e.preventDefault();

      let productImg =
        this.parentNode.parentNode.parentNode.parentNode.parentNode.firstElementChild.firstElementChild.getAttribute(
          "src"
        );
      let brandName = this.parentNode.parentNode.parentNode.parentNode.nextElementSibling.firstElementChild.firstElementChild.innerText;
      let productName =
        this.parentNode.parentNode.parentNode.parentNode.nextElementSibling.firstElementChild.nextElementSibling.firstElementChild
          .innerText;
      let productPrice =
        this.parentNode.parentNode.parentNode.parentNode.nextElementSibling.lastElementChild.innerText;
      let productId = parseInt(
        this.parentNode.parentNode.parentNode.parentNode.parentNode.getAttribute("product-id")
      );

      let existProduct = products.find((m) => m.id == productId);

      if ((existProduct != undefined)) {
        existProduct.count += 0;
      } else {
        products.push({
          id: productId,
          image: productImg,
          brand: brandName,
          name: productName,
          price: productPrice,
          count: 1,
        });
      }

      localStorage.setItem("products", JSON.stringify(products));

      heartCount.innerText = getHeartCount(products);
      heartCount2.innerText = getHeartCount(products);
    });
  });

  heartCount.innerText = getHeartCount(products);
  heartCount2.innerText = getHeartCount(products);

  function getHeartCount(heartCount) {
    let count = 0;
    for (const heart of heartCount) {
      count += heart.count;
    }
    return count;
  }
});
