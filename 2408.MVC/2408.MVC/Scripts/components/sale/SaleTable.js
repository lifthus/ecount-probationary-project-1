﻿import { SelectSalesRequestDTO, selectSales } from "../../api/sales.js";
import { API_SALE_DELETE_PATH, API_SALE_SELECT_PATH, SALE_CREATE_POPUP_PATH, SALE_PATH, SALE_UPDATE_POPUP_PATH } from "../../constant.js";
import { openPopup } from "../../common.js";

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
        this.REMARKS = query.get('REMARKS') || '';
        this.pageSize = Number(query.get('pageSize')) || 10;
        this.pageNo = Number(query.get('pageNo')) || 1;


        const req = new SelectSalesRequestDTO();
        req.IO_DATE_start = this.IO_DATE_start;
        req.IO_DATE_end = this.IO_DATE_end;
        req.PROD_CD_list = this.PROD_CD_list;
        req.IO_DATE_NO_ord = this.IO_DATE_NO_ord;
        req.PROD_CD_ord = this.PROD_CD_ord;
        req.REMARKS = this.REMARKS;
        req.pageSize = this.pageSize;
        req.pageNo = this.pageNo;

        const resp = await selectSales(req);
        this.pageSize = resp.pageSize;
        this.totalCount = resp.totalCount;

        this.render();
        this.renderTableContent(resp.sales);

        const checkAllBox = this.querySelector("#all-sales-check-box");
        checkAllBox.addEventListener("click", (e) => {
            const allCheckboxes = this.querySelectorAll("input[type='checkbox']");
            allCheckboxes.forEach((checkbox) => {
                checkbox.checked = e.target.checked;
            });
        });

        const delBtn = this.querySelector("#sales-delete-btn");
        delBtn.addEventListener("click", async (e) => {
            const boxes = this.querySelectorAll("[name='sale-checkbox']");
            for (const b of boxes) {
                if (!b.checked) {
                    continue;
                }
                const COM_CODE = b.dataset.comCode;
                const IO_DATE = b.dataset.ioDate;
                const IO_NO = b.dataset.ioNo;
                const params = new URLSearchParams({
                    COM_CODE, IO_DATE, IO_NO
                });
                const resp = await fetch(API_SALE_DELETE_PATH + `?${params.toString()}`, {
                    method: 'DELETE',
                    headers: { 'Content-Type': 'application/json' },
                });
                if (!resp.ok) {
                    alert($`${IO_DATE}-${IO - NO} 삭제 실패`);
                }
                location.reload();
            }
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
                    <th class="w-10px"><input type="checkbox" id="all-sales-check-box" /></th>
                    <th id="ord-io-date-no">전표일자/번호 ${this.IO_DATE_NO_ord == 0 ? '⏺' : this.IO_DATE_NO_ord > 0 ? '🔼' : '🔽'}</th>
                    <th id="ord-prod-cd">품목코드 ${this.PROD_CD_ord == 0 ? '⏺' : this.PROD_CD_ord > 0 ? '🔼' : '🔽'}</th>
                    <th>품목명</th>
                    <th>수량</th>
                    <th>단가</th>
                    <th>적요</th>
                </tr>
            </thead>
            <tbody id="sale-table-body"></tbody>
        </table>
        <button id="create-new-sale-btn" class="blue-btn">신규</button>
        <button class="gray-btn" id="sales-delete-btn">선택삭제</button>
        `;

        this.querySelector("#create-new-sale-btn").addEventListener("click", () => { openPopup(SALE_CREATE_POPUP_PATH); });

        this.querySelector("#ord-io-date-no").addEventListener("click", () => {
            const urlQuery = new URLSearchParams(window.location.search);
            urlQuery.set('IO_DATE_NO_ord', (this.IO_DATE_NO_ord + 2) % 3 - 1);
            window.location.href = SALE_PATH + `?${urlQuery.toString()}`;
        });
        this.querySelector("#ord-prod-cd").addEventListener("click", () => {
            const urlQuery = new URLSearchParams(window.location.search);
            urlQuery.set('PROD_CD_ord', (this.PROD_CD_ord + 2) % 3 - 1);
            window.location.href = SALE_PATH + `?${urlQuery.toString()}`;
        });
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
                <input type="checkbox" name="sale-checkbox" data-com-code="${sale.Key.COM_CODE}" data-io-date="${sale.Key.IO_DATE}" data-io-no="${sale.Key.IO_NO}" />
                </td>
                <td class="bd-sm bd-solid bd-gray txt-mildblue hover:cursor-pointer"
                    data-com-code="${sale.Key.COM_CODE}"
                    data-io-date="${sale.Key.IO_DATE}"
                    data-io-no="${sale.Key.IO_NO}"
                    name="cell-io-date-no">
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

        this.querySelectorAll("[name='cell-io-date-no'")
            .forEach(c => {
                c.addEventListener("click", (e) => {
                    const comCode = e.target.dataset.comCode;
                    const ioDate = e.target.dataset.ioDate;
                    const ioNo = e.target.dataset.ioNo
                    openPopup(SALE_UPDATE_POPUP_PATH + `?COM_CODE=${comCode}&IO_DATE=${ioDate}&IO_NO=${ioNo}`);
                });
            });
    }
}