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

  let tableBody = document.querySelector(".card-body .table .table-body");

  addProductTable(products);

  showHeartCount();

  let deleteIcons = document.querySelectorAll(".delete-icon");

  deleteIcons.forEach((icon) => {
    icon.addEventListener("click", function () {
      Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!",
      }).then(function (result) {
        if (result.isConfirmed) {
          deleteProducts(icon);
        } else {
          return false;
        }
      });
    });
  });

  function deleteProducts(icon) {
    let id = parseInt(
      icon.parentNode.parentNode.firstElementChild.getAttribute("data-id")
    );

    products = products.filter((m) => m.id != id);

    localStorage.setItem("products", JSON.stringify(products));

    icon.parentNode.parentNode.remove();

    showHeartCount();
  }

  function addProductTable(products) {
    for (const product of products) {
      tableBody.innerHTML += `<tr>
            <td data-id = "${product.id}"><a asp-controller="ProductDetail" asp-action="Index" data-id = "${product.id}"><img src="${product.image}" width = "150px" height = "150px" alt=""></a> </td>
            <td>${product.brand}</td>
            <td>${product.name}</td>
            <td>${product.price}</td>
            <td>
                <i class="fa-solid fa-cart-shopping add-basket"></i>
                <i class="fa-solid fa-trash delete-icon"></i>
            </td>
        </tr>`;
    }
  }

  function showHeartCount() {
    heartCount.innerText = getHeartCount(products);
    heartCount2.innerText = getHeartCount(products);
  }
});
