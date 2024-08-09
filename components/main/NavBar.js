class NavBar extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: "open" });
    }
    connectedCallback() {
        const href = this.getAttribute("href");
        const content = this.innerHTML.trim() || href;

        this.shadowRoot.innerHTML = `
        <div class="p-md">
            <menu-button href="/pages/sales.html">판매 조회</menu-button>
            <menu-button href="/popups/items.html">품목 조회</menu-button>
        </div>
        `;
    }
}
customElements.define("nav-bar", NavBar);
