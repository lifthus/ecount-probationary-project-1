import { ITEM_CREATE_POPUP_PATH, ITEM_UPDATE_POPUP_PATH, ITEM_PATH, ITEM_SELECT_POPUP_PATH, PRODUCT_PATH } from "../../constant.js";
import { SelectProductRequestDTO, deleteItems, selectProducts } from "../../api/items.js"
import { openPopup } from "../../util.js";

export class ItemTable extends HTMLElement {
    constructor() {
        super();
    }
    async connectedCallback() {
        const urlQuery = new URLSearchParams(window.location.search);
        const PROD_CD = urlQuery.get("PROD_CD") || "";
        const PROD_NM = urlQuery.get("PROD_NM") || "";
        const ord_PROD_NM = Number(urlQuery.get("ord_PROD_NM")) || 0;
        const ACTIVE = Number(urlQuery.get("ACTIVE")) || 0;
        const pageSize = Number(urlQuery.get("pageSize")) || 10;
        const pageNo = Number(urlQuery.get("pageNo")) || 1;

        const selectProdReqDTO = new SelectProductRequestDTO();
        selectProdReqDTO.COM_CODE = '80000';
        selectProdReqDTO.PROD_CD = PROD_CD;
        selectProdReqDTO.PROD_NM = PROD_NM;
        selectProdReqDTO.ord_PROD_NM = ord_PROD_NM;
        selectProdReqDTO.ACTIVE = ACTIVE;
        selectProdReqDTO.pageSize = pageSize;
        selectProdReqDTO.pageNo = pageNo;
        const resp = await selectProducts(selectProdReqDTO);

        const maxSelect = Number(urlQuery.get("maxSelect")) || undefined;
        this.addEventListener("click", (e) => {
            const id = e.target.id;
            if (id === "all-items-check-box") {
                if (maxSelect) return;
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
            } else if (id === "close-button") {
                self.close();
            }
        });
        this.innerHTML = `
        <div class="flex p-sm gap-sm">
            <page-nav path="${PRODUCT_PATH}" pageSize="${resp.pageSize}" totalCount="${resp.totalCount}"/>
        </div>
        <table class="w-100 bd-sm bd-solid bd-gray bd-collapse">
            <thead>
                <tr>
                    <th class="w-10px bd-sm bd-solid bd-gray bg-whitesmoke">
                        <input type="checkbox" id="all-items-check-box" />
                    </th>
                    <th class="bd-sm bd-solid bd-gray bg-whitesmoke">품목코드</th>
                    <th id="ord-prod-nm" class="bd-sm bd-solid bd-gray bg-whitesmoke">품목명 ${ord_PROD_NM == 0 ? '⏺' : ord_PROD_NM > 0 ? '🔼' : '🔽'}</th>
                    <th class="bd-sm bd-solid bd-gray bg-whitesmoke">단가</th>
                    <th class="bd-sm bd-solid bd-gray bg-whitesmoke">작성일</th>
                    <th class="bd-sm bd-solid bd-gray bg-whitesmoke w-50px">수정</th>
                </tr>
            </thead>
            <tbody id="item-table-body"></tbody>
        </table>
        <div class="flex p-sm gap-sm">
            ${
                maxSelect
                    ? `
            <blue-button buttonId="item-apply-button">적용</blue-button>
                `
                    : ""
            }
            <gray-button buttonId="create-item-button">신규</gray-button>
            <gray-button buttonId="delete-items-button">선택삭제</gray-button>
            ${
                maxSelect
                    ? `
            <gray-button buttonId="close-button">닫기</gray-button>
                `
                    : ""
            }
        </div>
        `;
        this.renderTableContent(resp.products);
        const itemCheckboxes = this.querySelectorAll("[name='item-checkbox']");
        itemCheckboxes.forEach((cb) => {
            cb.addEventListener("click", (e) => {
                const allItemsCheckbox = this.querySelector("#all-items-check-box");
                const checkedItemCheckboxes = this.querySelectorAll("[name='item-checkbox']:checked");
                if (checkedItemCheckboxes.length > maxSelect) {
                    alert(`최대 ${maxSelect}개까지 선택 가능합니다.`);
                    e.target.checked = false;
                }
                if (e.target.checked) allItemsCheckbox.checked = true;
                if (checkedItemCheckboxes.length === 0) allItemsCheckbox.checked = false;
            });
        });

        // 품목명 정렬
        this.querySelector("#ord-prod-nm").addEventListener("click", () => {
            urlQuery.set('ord_PROD_NM', (ord_PROD_NM + 2) % 3 - 1);
            window.location.href = PRODUCT_PATH + `?${urlQuery.toString()}`;
        });

        // 아이템 선택 후 적용 시
        this.querySelector("#item-apply-button").addEventListener("click", () => {
            const checkedItemCheckboxes = this.querySelectorAll("[name='item-checkbox']:checked");
            const items = [];
            checkedItemCheckboxes.forEach((cb) => {
                items.push({ code: cb.dataset.itemCode, name: cb.dataset.itemName });
            });
            window.opener.onItemSelect(items);
            self.close();
        });
    }

    /**
     *
     * @param {import("../../api/items.js").ProductDTO[]} items
     */
    renderTableContent(items) {
        const itemTableBody = this.querySelector("#item-table-body");
        itemTableBody.innerHTML = `
        ${items.map(
                (item) => `
            <tr>
                <td class="bd-sm bd-solid bd-gray">
                <input type="checkbox" name="item-checkbox" data-com-cde="${item.COM_CODE}" data-prod-cd="${item.PROD_CD}" data-prod-nm="${item.PROD_NM}" />
                </td>
                <td class="bd-sm bd-solid bd-gray">
                    ${item.Key.PROD_CD}
                </td>
                <td class="bd-sm bd-solid bd-gray">
                    ${item.PROD_NM}
                </td>
                <td class="bd-sm bd-solid bd-gray">
                    ${item.PRICE}
                </td>
                <td class="bd-sm bd-solid bd-gray">
                    ${item.WRITE_DT}
                </td>
                <td class="bd-sm bd-solid bd-gray txt-center hover:cursor-pointer">
                    <a 
                        name="item-update-button"
                        data-prod-cd="${item.Key.PROD_CD}"
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
                const prodCd = e.target.dataset.prodCd;
                openPopup(`${ITEM_UPDATE_POPUP_PATH}?PROD_CD=${prodCd}`);
            });
        });
    }
}
