import { ITEM_PATH, SALE_PATH } from "../../constant.js";

export class SaleFilter extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const href = this.getAttribute("href");
        const content = this.innerHTML.trim() || href;

        const query = new URLSearchParams(window.location.search);
        const startDate = query.get("startDate") || "";
        const endDate = query.get("endDate") || "";
        const itemCodes = query.get("itemCodes") ? query.get("itemCodes").split(",") : [];
        const itemNames = query.get("itemNames") ? query.get("itemNames").split(",") : [];
        const briefs = query.get("briefs") || "";
        const page = Number(query.get("page")) || 1;

        this.innerHTML = `
          <form class="flex-col" id="item-search-form">
                <filter-date-range 
                    startDateId="start-date-value" 
                    endDateId="end-date-value"
                    defaultStartDate="${startDate}"
                    defaultEndDate="${endDate}"    
                >전표일자</filter-date-range>
                <filter-item-select
                    max="1"
                >품목</filter-item-select>
                <filter-input-text
                    filterId="filter-sale-briefs"
                    filterName="filter-sale-briefs"
                    defaultValue="${briefs}"
                >
                    적요
                </filter-input-text>
                <filter-item-div>
                    <blue-button buttonId="sale-search-button">검색</blue-button>
                </div>
            </form>
        `;
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
            itemList = [...itemList.map((it) => ({ code: it }))];
        };
    }
}
