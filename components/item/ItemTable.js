import { ITEM_CREATE_POPUP_PATH, ITEM_UPDATE_POPUP_PATH, ITEM_PATH } from "../../constant.js";
import { deleteItems, queryItems } from "../../scripts/api/items.js";
import { openPopup } from "../../scripts/util.js";

export class ItemTable extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const urlQuery = new URLSearchParams(window.location.search);
        const code = urlQuery.get("code") || "";
        const name = urlQuery.get("name") || "";
        const page = Number(urlQuery.get("page")) || 1;
        const resp = queryItems({ itemCode: code, itemName: name, page });
        const totalPages = resp.totalPages;
        this.addEventListener("click", (e) => {
            const id = e.target.id;
            if (id === "prev-page-button") {
                if (page === 1) {
                    location.href = `${ITEM_PATH}?code=${code}&name=${name}&page=1`;
                    return;
                }
                location.href = `${ITEM_PATH}?code=${code}&name=${name}&page=${page - 1}`;
            } else if (id === "next-page-button") {
                if (page === totalPages) {
                    location.href = `${ITEM_PATH}?code=${code}&name=${name}&page=${totalPages}`;
                    return;
                }
                location.href = `${ITEM_PATH}?code=${code}&name=${name}&page=${page + 1}`;
            } else if (id === "all-items-check-box") {
                const allCheckboxes = this.querySelectorAll("input[type='checkbox']");
                allCheckboxes.forEach((checkbox) => {
                    checkbox.checked = e.target.checked;
                });
            } else if (id === "create-item-button") {
                openPopup(ITEM_CREATE_POPUP_PATH);
            } else if (id === "delete-items-button") {
                const tbody = this.querySelector("#item-table-body");
                const checkedBoxNodes = tbody.querySelectorAll("input[type='checkbox']:checked");
                const codes = [];
                checkedBoxNodes.forEach((cb) => codes.push(cb.dataset.itemCode));
                deleteItems(codes);
                location.reload();
            }
        });
        this.innerHTML = `
        <div class="flex p-sm gap-sm">
            <gray-button buttonId="prev-page-button">< 이전</gray-button>
            <gray-button buttonId="next-page-button">다음 ></gray-button>
        </div>
        <table class="w-100 bd-sm bd-solid bd-gray bd-collapse">
            <thead>
                <tr>
                    <th class="w-10px bd-sm bd-solid bd-gray bg-whitesmoke">
                        <input type="checkbox" id="all-items-check-box" />
                    </th>
                    <th class="bd-sm bd-solid bd-gray bg-whitesmoke">품목코드</th>
                    <th class="bd-sm bd-solid bd-gray bg-whitesmoke">품목명</th>
                    <th class="bd-sm bd-solid bd-gray bg-whitesmoke w-50px">수정</th>
                </tr>
            </thead>
            <tbody id="item-table-body"></tbody>
        </table>
        <div class="flex p-sm gap-sm">
            <gray-button buttonId="create-item-button">신규</gray-button>
            <gray-button buttonId="delete-items-button">선택삭제</gray-button>
        </div>
        `;
        this.renderTableContent(resp.items);
    }

    /**
     *
     * @param {import("../../scripts/api/items.js").ItemDTO[]} items
     */
    renderTableContent(items) {
        const itemTableBody = this.querySelector("#item-table-body");
        itemTableBody.innerHTML = `
        ${items
            .map(
                (item) => `
            <tr>
                <td class="bd-sm bd-solid bd-gray">
                <input type="checkbox" data-item-code="${item.code}" data-item-name="${item.name}" />
                </td>
                <td class="bd-sm bd-solid bd-gray">
                    ${item.code}
                </td>
                <td class="bd-sm bd-solid bd-gray">
                    ${item.name}
                </td>
                <td class="bd-sm bd-solid bd-gray txt-center">
                    <a 
                        name="item-update-button"
                        data-item-code="${item.code}"
                        class="txt-mildblue">
                        수정
                    </a>
                </td>
            </tr>
            `
            )
            .join("")}
        `;
        const itemUpdateButtons = this.querySelectorAll("[name='item-update-button']");
        itemUpdateButtons.forEach((btn) => {
            btn.addEventListener("click", (e) => {
                const code = e.target.dataset.itemCode;
                openPopup(`${ITEM_UPDATE_POPUP_PATH}?code=${code}`);
            });
        });
    }
}
