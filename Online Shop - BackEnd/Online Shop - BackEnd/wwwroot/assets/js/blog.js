"use strict";
$(function () {
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

    $(document).on("click", ".categories", function (e) {
        e.preventDefault();
        $(this).next().next().slideToggle();
    });

    $(document).on("click", ".category li a", function (e) {
        e.preventDefault();
        let category = $(this).attr("data-id");
        let products = $(".product-item");

        products.each(function () {
            if (category == $(this).attr("data-id")) {
                $(this).parent().fadeIn();
            } else {
                $(this).parent().hide();
            }
        });
        if (category == "all") {
            products.parent().fadeIn();
        }
    });

  let products = [];

  if (localStorage.getItem("products") != null) {
    products = JSON.parse(localStorage.getItem("products"));
  }

  let heartCount = document.querySelector(".heart sup");
  let heartCount2 = document.querySelector("#scroll-section .heart sup");

  heartCount.innerText = getHeartCount(products);
  heartCount2.innerText = getHeartCount(products);

  function getHeartCount(heartCount) {
    let count = 0;
    for (const heart of heartCount) {
      count += heart.count;
    }
    return count;
    }

    $(document).on("keyup", "#search-box", function () {
        let inputVal = $(this).val().trim();

        $("#search-list-blog li").slice(0).remove();
        $.ajax({
            url: "blog/search",
            type: "Get",
            contentType: "application/x-www-form-urlencoded",
            data: {
                search: inputVal
            },

            success: function (res) {
                $("#search-list-blog").append(res);
            }
        });
    });
});
