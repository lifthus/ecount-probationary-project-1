export class FilterSelectedItem extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const code = this.getAttribute("code");
        const name = this.getAttribute("name");
        this.innerHTML = `
        <div class="bg-lightblue m-xs bdr-sm txt-white flex justify-center align-center">
            <div class="txt-sm">${code}</div>
            <div class="bd-sm bd-solid bd-white h-100"></div>
            <div class="txt-sm">${name}</div>
            <div class="bd-sm bd-solid bd-white h-100"></div>
            <button class="bdr-md p-0 txt-xl bd-none bg-lightblue" type="button">-</button>
        </div>
        `;
    }
}
