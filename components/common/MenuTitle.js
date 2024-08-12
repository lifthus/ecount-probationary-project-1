export class MenuTitle extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const content = this.innerHTML.trim() || href;
        this.innerHTML = `
          <div 
            class="p-sm bd-solid bd-md bd-gray"
          >
            ■ ${content}
        </div>
        `;
    }
}
