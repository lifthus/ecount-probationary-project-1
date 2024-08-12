import { ITEM_SELECT_POPUP_PATH } from "../../../constant.js";
import { openPopup } from "../../../scripts/util.js";

export class FilterItemSelect extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const maxItemSelect = this.getAttribute("max");
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
            <input placeholder="품목" name="sales-item-codes" />
        </div>
        `;
        const itemSelectButton = this.querySelector("#item-select-button");
        itemSelectButton.addEventListener("click", () => {
            openPopup(`${ITEM_SELECT_POPUP_PATH}?max=${selectLimit}`);
        });
    }
}
