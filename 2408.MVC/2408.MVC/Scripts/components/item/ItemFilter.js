import { PRODUCT_PATH } from "../../constant.js";

export class ItemFilter extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const href = this.getAttribute("href");
        const urlQuery = new URLSearchParams(window.location.search);
        const maxSelect = Number(urlQuery.get("maxSelect")) || undefined;

        const query = new URLSearchParams(window.location.search);
        const code = query.get("PROD_CD") || "";
        const name = query.get("PROD_NM") || "";
        const pageSize = Number(query.get("pageSIze")) || 10;

        this.innerHTML = `
          <form class="flex-col" id="item-search-form">
                <filter-input-text
                    filterId="filter-item-code"
                    filterName="filter-item-code"
                    defaultValue="${code}"
                >
                    품목 코드
                </filter-input-text>
                <filter-input-text
                    filterId="filter-item-name"
                    filterName="filter-item-name"
                    defaultValue="${name}"
                >
                    품목명
                </filter-input-text>
                <filter-item-div>
                    <blue-button buttonId="item-search-button">검색</blue-button>
                </div>
            </form>
        `;

        this.addEventListener("click", (event) => {
            if (event.target.id === "item-search-button") {
                const code = document.getElementById("filter-item-code").value;
                const name = document.getElementById("filter-item-name").value;
                const newPageSize = Number(document.getElementById("page-size-input").value) || pageSize;
                const targetPath = maxSelect ? PRODUCT_SELECT_POPUP_PATH : PRODUCT_PATH;
                urlQuery.set("PROD_CD", code);
                urlQuery.set("PROD_NM", name);
                urlQuery.set("pageSize", newPageSize);
                window.location.href = targetPath + `?${urlQuery.toString()}`;
            }
        });
    }
}
