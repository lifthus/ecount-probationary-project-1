import { ITEM_PATH, SALE_PATH } from "../../../constant.js";

export class NavBar extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        this.innerHTML = `
        <div class="p-md flex gap-md">
            <menu-button href="${SALE_PATH}">🧾 판매 조회</menu-button>
            <menu-button href="${ITEM_PATH}">📦 품목 조회</menu-button>
        </div>
        `;
    }
}
