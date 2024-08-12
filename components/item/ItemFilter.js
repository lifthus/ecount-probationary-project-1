import { ITEM_PATH } from "../../constant.js";

export class ItemFilter extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const href = this.getAttribute("href");
        const content = this.innerHTML.trim() || href;

        this.addEventListener("click", (event) => {
            if (event.target.id === "item-search-button") {
                const code = document.getElementById("filter-item-code").value;
                const name = document.getElementById("filter-item-name").value;
                window.location.href = ITEM_PATH + `?code=${code || ""}&name=${name || ""}`;
            }
        });

        const query = new URLSearchParams(window.location.search);
        const code = query.get("code") || "";
        const name = query.get("name") || "";

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
    }
}
