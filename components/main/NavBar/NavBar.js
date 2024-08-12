export class NavBar extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        this.innerHTML = `
        <div class="p-md flex gap-md">
            <menu-button href="/pages/sales.html">🧾 판매 조회</menu-button>
            <menu-button href="/popups/items.html">📦 품목 조회</menu-button>
        </div>
        `;
    }
}
