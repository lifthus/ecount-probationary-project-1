import { API_SALE_SELECT_PATH, SALE_PATH } from "../../constant.js";

export class SaleTable extends HTMLElement {
    constructor() {
        super();
    }

    /* 쿼리 상태 */
    startDate = "";
    endDate = "";
    itemList = [];
    briefs = "";
    page = 1;

    connectedCallback() {
        /* 쿼리 파라미터에서 조회 쿼리 값 가져오기 */
        const query = new URLSearchParams(window.location.search);
        this.startDate = query.get("startDate") || "";
        this.endDate = query.get("endDate") || "";
        // TODO: 일단 쿼리에 아이템 코드/명 무지성으로 넣음. 추후에 가능하면 좀 더 깔끔하게 수정
        const itemCodes = query.get("itemCodes") ? query.get("itemCodes").split(",") : [];
        const itemNames = query.get("itemNames") ? query.get("itemNames").split(",") : [];
        this.itemList = itemCodes.map((code, idx) => ({ code, name: itemNames[idx] }));
        this.briefs = query.get("briefs") || "";
        this.page = Number(query.get("page")) || 1;

        this.render();

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
          <form class="flex-col" id="item-search-form">
                <filter-date-range 
                    startDateId="start-date-value" 
                    endDateId="end-date-value"
                    defaultStartDate="${this.startDate}"
                    defaultEndDate="${this.endDate}"    
                >전표일자</filter-date-range>
                <filter-item-select
                    maxSelect="3"
                    itemList='${JSON.stringify(this.itemList)}'
                >품목</filter-item-select>
                <filter-input-text
                    filterId="filter-sale-briefs"
                    filterName="filter-sale-briefs"
                    defaultValue="${this.briefs}"
                >
                    적요
                </filter-input-text>
                <filter-item-div>
                    <blue-button buttonId="sale-search-button">검색</blue-button>
                </div>
            </form>
        `;
    }
}