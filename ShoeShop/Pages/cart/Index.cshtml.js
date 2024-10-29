//$(document).ready(function () {
//    function updateTotalDisplay() {
//        let totalMoney = formatMoney(total(), 0, ",", ".");
//        $(".total-sum").text(totalMoney);
//    }

//    function total() {
//        let totalMoney = 0;
//        $(".total-money").each(function () {
//            let money = parseFloat($(this).attr("data-money")) || 0;
//            totalMoney += money;
//        });
//        return totalMoney;
//    }

//    function updateItemDisplay(cartItem, qty, price) {
//        let money = qty * price;
//        cartItem.find(".total").text(formatMoney(money, 0, ",", "."));
//        cartItem.find(".total-money").attr("data-money", money);
//        updateTotalDisplay();
//    }

//    function formatMoney(amount, decimalCount = 2, decimal = ".", thousands = ",") {
//        try {
//            decimalCount = Math.abs(decimalCount);
//            const negativeSign = amount < 0 ? "-" : "";
//            let i = parseInt(Math.abs(Number(amount) || 0).toFixed(decimalCount)).toString();
//            let j = (i.length > 3) ? i.length % 3 : 0;

//            return negativeSign +
//                (j ? i.substr(0, j) + thousands : '') +
//                i.substr(j).replace(/(\d{3})(?=\d)/g, "$1" + thousands) +
//                (decimalCount ? decimal + Math.abs(amount - i).toFixed(decimalCount).slice(2) : "");
//        } catch (e) {
//            console.log(e);
//        }
//    }

//    $(".qty-btn.fa-minus").click(function () {
//        let cartItem = $(this).closest(".cart-item");
//        let input = cartItem.find(".qty-input");
//        let qty = Math.max(1, parseInt(input.val()) - 1);
//        input.val(qty);

//        let id = cartItem.data("id");
//        let price = parseFloat(cartItem.find(".price").text().replace(/,/g, ""));

//        updateCart(id, qty, function () {
//            updateItemDisplay(cartItem, qty, price);
//        });
//    });

//    $(".qty-btn.fa-plus").click(function () {
//        let cartItem = $(this).closest(".cart-item");
//        let input = cartItem.find(".qty-input");
//        let qty = parseInt(input.val()) + 1;
//        input.val(qty);

//        let id = cartItem.data("id");
//        let price = parseFloat(cartItem.find(".price").text().replace(/,/g, ""));

//        updateCart(id, qty, function () {
//            updateItemDisplay(cartItem, qty, price);
//        });
//    });

//    $(".remove-btn").click(function () {
//        let cartItem = $(this).closest(".cart-item");
//        let id = cartItem.data("id");

//        if (confirm("Bạn có chắc chắn muốn xóa sản phẩm này khỏi giỏ hàng?")) {
//            removeCartItem(id, function () {
//                cartItem.remove();
//                updateTotalDisplay();
//            });
//        }
//    });

//    function updateCart(id, quantity, callback) {
//        $.ajax({
//            url: `/cart/Update`,
//            method: "GET",
//            data: { id: id, quantity: quantity },
//            success: function () {
//                if (callback) callback();
//            },
//            error: function (error) {
//                console.error("Lỗi khi cập nhật giỏ hàng:", error);
//            }
//        });
//    }

//    function removeCartItem(id, callback) {
//        $.ajax({
//            url: `/cart/Remove`,
//            method: "GET",
//            data: { id: id },
//            success: function () {
//                if (callback) callback();
//            },
//            error: function (error) {
//                console.error("Lỗi khi xóa sản phẩm:", error);
//            }
//        });
//    }
//});
