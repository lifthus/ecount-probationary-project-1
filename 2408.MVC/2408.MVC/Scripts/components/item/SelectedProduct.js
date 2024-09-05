export class SelectedProduct extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const cancelId = this.getAttribute("cancelId");
        const code = this.getAttribute("code");
        const name = this.getAttribute("name");
        this.innerHTML = `
          <div class="flex bdr-sm bd-solid bd-sm bd-lightgray bg-lightblue txt-white justify-center align-center">
            ${name}
            |
            <div class="txt-sm">${code}</div>
            <div id="${cancelId}" class="justify-center align-center bdr-full bd-solid bd-sm p-xs w-10px h-10px hover:cursor-pointer">x</div>
          </div>
        `;
    }
}
