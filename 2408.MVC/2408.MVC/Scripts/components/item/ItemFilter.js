import { ITEM_PATH, ITEM_SELECT_POPUP_PATH } from "../../constant.js";

export class ItemFilter extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const href = this.getAttribute("href");
        const urlQuery = new URLSearchParams(window.location.search);
        const maxSelect = Number(urlQuery.get("maxSelect")) || undefined;

        this.addEventListener("click", (event) => {
            if (event.target.id === "item-search-button") {
                const code = document.getElementById("filter-item-code").value;
                const name = document.getElementById("filter-item-name").value;
                const targetPath = maxSelect ? ITEM_SELECT_POPUP_PATH : ITEM_PATH;
                window.location.href = targetPath + `?code=${code || ""}&name=${name || ""}&maxSelect=${maxSelect || ""}`;
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
