export class ErpBody extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const content = this.innerHTML.trim() || href;
        this.innerHTML = `
          <div 
            class="py-md"
          >
            ${content}
        </div>
        `;
    }
}
