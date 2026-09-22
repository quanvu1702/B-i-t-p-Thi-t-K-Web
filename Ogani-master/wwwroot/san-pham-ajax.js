document.addEventListener("DOMContentLoaded", function () {
    const container = document.getElementById("danh-sach-san-pham");

    if (!container) return;

    let requestNumber = 0;

    const priceFormatter = new Intl.NumberFormat("vi-VN", {
        style: "currency",
        currency: "VND"
    });

    function createElement(tag, className, text) {
        const element = document.createElement(tag);
        element.className = className || "";

        if (text !== undefined) {
            element.textContent = text;
        }

        return element;
    }

    function showMessage(text, isError = false) {
        const message = createElement(
            "p",
            isError ? "col-12 text-danger" : "col-12",
            text
        );

        container.replaceChildren(message);
    }

    function createProductCard(product) {
        // Bỏ dấu cách đệm cuối các cột char trong database.
        const maSp = (product.maSp || "").trim();
        const tenSp = (product.tenSp || maSp).trim();
        const tenAnh = (product.anhDaiDien || "").trim();

        const detailUrl = new URL(
            container.dataset.detailUrl,
            window.location.origin
        );
        detailUrl.searchParams.set("maSp", maSp);

        const column = createElement(
            "div",
            "col-lg-3 col-md-4 col-sm-6"
        );

        const card = createElement("div", "featured__item");
        const imageLink = createElement("a");
        imageLink.href = detailUrl.href;
        imageLink.style.display = "block";

        const imageBox = createElement("div");
        imageBox.style.cssText =
            "height:270px;display:flex;align-items:center;" +
            "justify-content:center;background:#f5f5f5;";

        if (tenAnh) {
            const image = document.createElement("img");

            image.src = container.dataset.imageUrl
                + encodeURIComponent(tenAnh);

            image.alt = tenSp;
            image.loading = "lazy";
            image.style.cssText =
                "width:100%;height:270px;object-fit:contain;";

            image.addEventListener("error", function () {
                imageBox.replaceChildren(
                    createElement("span", "text-muted", "Chưa có ảnh")
                );
            }, { once: true });

            imageBox.appendChild(image);
        } else {
            imageBox.appendChild(
                createElement("span", "text-muted", "Chưa có ảnh")
            );
        }

        imageLink.appendChild(imageBox);

        const info = createElement("div", "featured__item__text");
        const heading = document.createElement("h6");
        const nameLink = createElement("a", "", tenSp);

        nameLink.href = detailUrl.href;
        heading.appendChild(nameLink);

        const price = document.createElement("h5");

        price.textContent = product.giaNhoNhat == null
            ? "Liên hệ"
            : priceFormatter.format(product.giaNhoNhat);

        info.append(heading, price);
        card.append(imageLink, info);
        column.appendChild(card);

        return column;
    }

    document.addEventListener("click", async function (event) {
        const link = event.target.closest("a.js-loai-san-pham");

        if (!link) return;

        if (event.ctrlKey || event.metaKey ||
            event.shiftKey || event.altKey) {
            return;
        }

        event.preventDefault();

        const currentRequest = ++requestNumber;
        const maLoai = (link.dataset.maLoai || "").trim();

        const url = new URL(
            container.dataset.apiUrl,
            window.location.origin
        );

        if (maLoai) {
            url.searchParams.set("maLoai", maLoai);
        }

        showMessage("Đang tải sản phẩm...");

        try {
            const response = await fetch(url, {
                headers: { "Accept": "application/json" }
            });

            if (currentRequest !== requestNumber) return;

            if (response.status === 401) {
                window.location.href = container.dataset.loginUrl;
                return;
            }

            if (!response.ok) {
                throw new Error("Không tải được sản phẩm.");
            }

            const products = await response.json();

            if (currentRequest !== requestNumber) return;

            if (!Array.isArray(products)) {
                throw new Error("Dữ liệu trả về không hợp lệ.");
            }

            const fragment = document.createDocumentFragment();

            const title = createElement(
                "h4",
                "col-12 mb-4",
                link.textContent.trim()
            );

            fragment.appendChild(title);

            if (products.length === 0) {
                fragment.appendChild(
                    createElement(
                        "p",
                        "col-12",
                        "Loại này chưa có sản phẩm."
                    )
                );
            } else {
                products.forEach(function (product) {
                    fragment.appendChild(createProductCard(product));
                });
            }

            container.replaceChildren(fragment);
        } catch (error) {
            if (currentRequest !== requestNumber) return;

            showMessage(
                "Không tải được sản phẩm. Bạn hãy chọn lại loại sản phẩm.",
                true
            );
        }
    });
});