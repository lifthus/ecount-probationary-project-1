class NavBar extends HTMLElement {
    constructor() {
        super();
        this.attachShadow({ mode: "open" });
    }
    connectedCallback() {
        const href = this.getAttribute("href");
        const content = this.innerHTML.trim() || href;

        this.shadowRoot.innerHTML = `
          <a href="${href}">${content}</a>
        `;
    }
}
customElements.define("nav-bar", NavBar);
