import { SelectSalesRequestDTO, selectSales } from "../../api/sales.js";
import { API_SALE_SELECT_PATH, SALE_PATH } from "../../constant.js";

export class SaleTable extends HTMLElement {
    constructor() {
        super();
    }

    /* 쿼리 상태 */
    IO_DATE_start = '';
    IO_DATE_end = '';
    PROD_CD_list = [];
    IO_DATE_NO_ord = 0;
    PROD_CD_ord = 0;
    REMARKS = '';
    pageSize = 10;
    pageNo = 1;

    totalCount = 0;

    async connectedCallback() {
        /* 쿼리 파라미터에서 조회 쿼리 값 가져오기 */
        const query = new URLSearchParams(window.location.search);
        this.IO_DATE_start = query.get('IO_DATE_start') || '1900/01/01';
        this.IO_DATE_end = query.get('IO_DATE_end') || '9999/12/31';
        let q_PROD_CD_list = query.get('PROD_CD_list');
        this.PROD_CD_list = q_PROD_CD_list ? q_PROD_CD_list.split(',') : [];
        this.IO_DATE_NO_ord = Number(query.get('IO_DATE_NO_ord')) || 0;
        this.PROD_CD_ord = Number(query.get('PROD_CD_ord')) || 0;
        this.pageSize = Number(query.get('pageSize')) || 10;
        this.pageNo = Number(query.get('pageNo')) || 1;


        const req = new SelectSalesRequestDTO();
        req.IO_DATE_start = this.IO_DATE_start;
        req.IO_DATE_end = this.IO_DATE_end;
        req.PROD_CD_list = this.PROD_CD_list;
        req.IO_DATE_NO_ord = this.IO_DATE_NO_ord;
        req.PROD_CD_ord = this.PROD_CD_ord;
        req.pageSize = this.pageSize;
        req.pageNo = this.pageNo;

        const resp = await selectSales(req);
        this.pageSize = resp.pageSize;
        this.totalCount = resp.totalCount;

        this.render();
        this.renderTableContent(resp.sales);

        const saleSearchButton = this.querySelector("#sale-search-button");
        saleSearchButton.addEventListener("click", () => {
            const startDateValue = this.querySelector("#start-date-value").value;
            const endDateValue = this.querySelector("#end-date-value").value;
            let itemList = [];
            const briefs = this.querySelector("#filter-sale-briefs").value;
            window.location.href =
                `${SALE_PATH}?startDate=${startDateValue}&endDate=${endDateValue}&briefs=${briefs}` +
                `&itemCodes=${itemList.map((it) => it.code).join(",")}&itemNames=${itemList.map((it) => it.name).join(",")}` +
                `&page=1`;
        });
        window.onItemSelect = (itemList) => {
            this.itemList = itemList;
            this.render();
        };
    }

    render() {
        this.innerHTML = `
        <div class="flex p-sm gap-sm">
            <page-nav path="${SALE_PATH}" pageSize="${this.pageSize}" totalCount="${this.totalCount}"/>
        </div>
          <table class="bordered-table">
            <thead>
                <tr>
                    <th class="w-10px"><input type="checkbox" id="check-whole" onclick="onCheckAll()" /></th>
                    <th>전표일자/번호</th>
                    <th>품목코드</th>
                    <th>품목명</th>
                    <th>수량</th>
                    <th>단가</th>
                    <th>적요</th>
                </tr>
            </thead>
            <tbody id="sale-table-body"></tbody>
        </table>
        <button class="blue-btn" onclick="openPopup('/popups/sale-create-input.html');">신규</button>
        <button class="gray-btn" onclick="onDeleteSales()">선택삭제</button>
        `;
    }

    /**
     *
     * @param {import("../../api/sales.js").SaleDTO[]} sales
     */
    renderTableContent(sales) {
        const saleTableBody = this.querySelector("#sale-table-body");
        saleTableBody.innerHTML = `
        ${sales.map(
            (sale) => `
            <tr>
                <td class="bd-sm bd-solid bd-gray">
                <input type="checkbox" name="item-checkbox" data-com-code="${sale.Key.COM_CODE}" data-io-date="${sale.Key.IO_DATE}" data-io-no="${sale.Key.IO_NO}" />
                </td>
                <td class="bd-sm bd-solid bd-gray">
                    ${sale.Key.IO_DATE}-${sale.Key.IO_NO}
                </td>
                <td class="bd-sm bd-solid bd-gray">
                    ${sale.PROD_CD}
                </td>
                <td class="bd-sm bd-solid bd-gray">
                    ${sale.PROD_NM}
                </td>
                <td class="bd-sm bd-solid bd-gray">
                    ${sale.QTY}
                </td>
                <td class="bd-sm bd-solid bd-gray">
                    ${sale.UNIT_PRICE}
                </td>
                <td class="bd-sm bd-solid bd-gray">
                    ${sale.REMARKS}
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