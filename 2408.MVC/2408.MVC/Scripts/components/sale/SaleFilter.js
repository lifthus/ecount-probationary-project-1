import { openPopup } from "../../common.js";
import { PRODUCT_SELECT_POPUP_PATH, SALE_PATH } from "../../constant.js";

export class SaleFilter extends HTMLElement {
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

    selectedProducts = [];

    connectedCallback() {
        /* 쿼리 파라미터에서 조회 쿼리 값 가져오기 */
        const query = new URLSearchParams(window.location.search);
        this.IO_DATE_start = query.get('IO_DATE_start') || '1900/01/01';
        this.IO_DATE_end = query.get('IO_DATE_end') || '9999/12/31';
        const q_PROD_CD_list = query.get('PROD_CD_list');
        this.PROD_CD_list = q_PROD_CD_list ? q_PROD_CD_list.split(',') : [];
        const q_PROD_NM_list = query.get('PROD_NM_list');
        const nm_list = q_PROD_NM_list ? q_PROD_NM_list.split(',') : [];
        this.selectedProducts = this.PROD_CD_list.map((cd,idx)=> ({ PROD_CD: cd, PROD_NM: nm_list[idx]||'-'}));
        this.IO_DATE_NO_ord = Number(query.get('IO_DATE_NO_ord')) || 0;
        this.PROD_CD_ord = Number(query.get('PROD_CD_ord')) || 0;
        this.REMARKS = query.get('REMARKS') || '';
        this.pageSize = Number(query.get('pageSize')) || 10;
        this.pageNo = Number(query.get('pageNo')) || 1;

        this.render();
    }

    render() {
        this.innerHTML = `
          <form class="flex-col" id="item-search-form">
                <filter-date-range 
                    startDateId="start-date-value" 
                    endDateId="end-date-value"
                    defaultStartDate="${this.IO_DATE_start}"
                    defaultEndDate="${this.IO_DATE_end}"    
                >전표일자</filter-date-range>
                <div class="flex bg-whitesmoke bd-solid bd-sm p-xs">
                    <label class="w-100px">
                        품목
                    </label>
                    <button 
                        id="product-select-button"
                        type="button"
                    >
                        🔍
                    </button>
                    ${this.selectedProducts.map((p,i) => {
                        return `
                        <selected-product cancelId="cancel-selected-prod-${i}" code="${p.PROD_CD}" name="${p.PROD_NM}"></selected-product>
                        `;
                    }).join('')}
                    <input id="prod-popper" type="text" class="bg-whitesmoke bd-none w-50px focus:outline-none"></input>
                </div>
                <filter-input-text
                    filterId="filter-sale-briefs"
                    filterName="filter-sale-briefs"
                    defaultValue="${this.REMARKS}"
                >
                    적요
                </filter-input-text>
                <filter-item-div>
                    <blue-button buttonId="sale-search-button">검색</blue-button>
                </div>
            </form>
        `;
        this.querySelector("#prod-popper").addEventListener("keydown", e => {
            if (e.key === 'Backspace') {
                this.selectedProducts.pop();
                this.render();
                this.querySelector("#prod-popper").focus();
            }
        })
        const saleSearchButton = this.querySelector("#sale-search-button");
        saleSearchButton.addEventListener("click", () => {
            const qs = new URLSearchParams(window.location.search);
            const startDateValue = this.querySelector("#start-date-value").value;
            const endDateValue = this.querySelector("#end-date-value").value;
            qs.set('IO_DATE_start', startDateValue);
            qs.set('IO_DATE_end', endDateValue);
            qs.set('PROD_CD_list', `${this.selectedProducts.map(p => p.PROD_CD).join(',')}`);
            qs.set('PROD_NM_list', `${this.selectedProducts.map(p => p.PROD_NM).join(',')}`)
            const remarks = this.querySelector('#filter-sale-briefs').value;
            qs.set('REMARKS', remarks)
            window.location.href = SALE_PATH + `?${qs.toString()}`
        });
        window.onApplyProducts = (products) => {
            this.selectedProducts = products;
            this.render();
        };
        this.addEventListener("click", (e) => {
            const targetId = e.target.id;
            this.selectedProducts.forEach((p, i) => {
                if (targetId === 'cancel-selected-prod-' + i) {
                    this.selectedProducts = this.selectedProducts.filter((p, idx) => idx !== i);
                    this.render();
                }
            });
            if (targetId === 'product-select-button') {
                openPopup(PRODUCT_SELECT_POPUP_PATH + '?maxSelect=3', 700);
            }
        });
    }
}
