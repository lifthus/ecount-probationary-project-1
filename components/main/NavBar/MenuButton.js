export class MenuButton extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const href = this.getAttribute("href");
        const content = this.innerHTML.trim() || href;

        this.innerHTML = `
          <div 
            class="bd-solid bd-md bd-lightgray bdr-sm p-md bg-darkwhite hover:bg-lightgray"
            onclick="location.href='${href}'">${content}</div>
        `;
    }
}
