import { ITEM_SELECT_POPUP_PATH } from "../../../constant.js";
import { openPopup } from "../../../util.js";

export class FilterItemSelect extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const maxItemSelect = this.getAttribute("maxSelect");
        const itemListStr = this.getAttribute("itemList");
        const itemList = itemListStr ? JSON.parse(itemListStr) : [];
        const selectLimit = Number(maxItemSelect) || 1;
        this.innerHTML = `
        <div class="flex bg-whitesmoke bd-solid bd-sm p-xs">
            <label class="w-100px">
                품목
            </label>
            <button 
                id="item-select-button"
                type="button"
            >
                🔍
            </button>
            ${itemList
                .map((item) => {
                    return `
                <filter-selected-item
                    code="${item.code}"
                    name="${item.name}"
                ></filter-selected-item>
                `;
                })
                .join("")}
        </div>
        `;
        const itemSelectButton = this.querySelector("#item-select-button");
        itemSelectButton.addEventListener("click", () => {
            openPopup(`${ITEM_SELECT_POPUP_PATH}?maxSelect=${selectLimit}`);
        });
    }
}
