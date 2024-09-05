import { PRODUCT_SELECT_POPUP_PATH } from '../../constant.js';
import { openPopup } from '../../common.js';
export class CreateSale extends HTMLElement {
    constructor() {
        super();
    }

    selectedItem = null;

    connectedCallback() {
        this.render();

        window.onApplyProducts = (products) => {
            if (products.length > 0) {
                this.selectedItem = products[0];
                this.render();
                this.querySelector("input[name='create-sale-price']").value = products[0].PRICE;
            }
        };
    }

    render() {
        this.innerHTML = `
          <div class="basic-title">■ 판매입력</div>
        <form class="filter-box" onsubmit="return onSubmit()" id="sale-create-form">
            <div>
                <div class="filter-item">
                    <div class="flex-box">
                        <label class="w-100px">전표일자</label>
                            <select id = "create-sale-year" name = "create-sale-year">
                                ${Array.from({ length: 50 }, (_, i) => {
            const thisYear = new Date().getFullYear() - i;
            const curYear = new Date().getFullYear();
            return `<option value="${thisYear}" ${thisYear === curYear ? 'selected' : ''}>${new Date().getFullYear() - i}</option>`;
        }).join("")
            }
                            </select >
                            /
                            <select id = "create-isale-month" name = "create-sale-month" >
                                ${Array.from({ length: 12 }, (_, i) => {
                const thisMonth = i + 1;
                const curMonth = new Date().getMonth() + 1;
                return `<option value="${thisMonth}" ${thisMonth === curMonth ? 'selected' : ''}>${i + 1}</option>`;
            }).join("")
            }
                            </select >
                            /
                            <select id = "create-sale-day" name = "create-sale-day" >
                                ${Array.from({ length: 31 }, (_, i) => {
                const thisDay = i + 1;
                const curDay = new Date().getDate();
                return `<option value="${thisDay}" ${thisDay === curDay ? 'selected' : ''}>${i + 1}</option>`;
            }).join("")
            }
                            </select >
                    </div>
                </div>
                <div class="filter-item">
                    <label class="w-100px">품목</label>
                    <div class="bg-darkwhite bdr-sm bd-solid bd-sm bd-lightgray hover:cursor-pointer" id="product-selection">🔍</div>
                    ${this.selectedItem ?
                `<selected-product cancelId="cancel-selected-prod" code="${this.selectedItem.PROD_CD}" name="${this.selectedItem.PROD_NM}"></selected-product>`
            : ``}
                    <input id="prod-popper" type="text" class="bg-whitesmoke bd-none w-50px focus:outline-none"></input>
                </div>
                <div class="filter-item">
                    <label class="w-100px">수량</label>
                    <input placeholder="수량" name="create-sale-qty" />
                </div>
                <div class="filter-item">
                    <label class="w-100px">단가</label>
                    <input placeholder="단가" name="create-sale-price" />
                </div>
                <div class="filter-item">
                    <label class="w-100px">적요</label>
                    <input placeholder="적요" name="create-sale-remarks" />
                </div>
            </div>
            <div class="flex-box">
                <button class="blue-btn" type="button" id="create-sale-btn">저장</button>
                <button class="gray-btn" type="reset" id="create-sale-reset">다시작성</button>
                <button class="gray-btn" type="button" onclick="self.close()">닫기</button>
            </div>
        </form>
        `;
        this.querySelector("#prod-popper").addEventListener("keydown", e => {
            if (e.key === 'Backspace') {
                this.selectedItem = null;
                this.render();
                this.querySelector("#prod-popper").focus();
            }
        });

        this.querySelector("#create-sale-reset").addEventListener("click", () => {
            this.selectedItem = null;
            this.render();
        })

        const prodSelectBtn = this.querySelector("#product-selection");
        prodSelectBtn.addEventListener("click", () => { openPopup(PRODUCT_SELECT_POPUP_PATH + '?maxSelect=1', 700) });

        const saveBtn = this.querySelector('#create-sale-btn');
        saveBtn.addEventListener("click", this.onSave)

        this.addEventListener("click", (e) => {
            const targetId = e.target.id;
            if (targetId == 'cancel-selected-prod') {
                this.selectedItem = null;
                this.render();
            }
        });
    }

    onSave = async () => {
        const form = document.getElementById("sale-create-form");
        const formData = new FormData(form);
        const saleYear = formData.get("create-sale-year");
        const saleMonth = formData.get("create-sale-month");
        const saleDay = formData.get("create-sale-day");
        console.log("AH", this.selectedItem);
        const saleProdCd = this.selectedItem.PROD_CD;
        const saleQty = formData.get('create-sale-qty');
        const salePrice = formData.get('create-sale-price');
        const saleRemarks = formData.get('create-sale-remarks');

        const resp = await fetch("/api/sale", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                Key: { COM_CODE: '80000', IO_DATE: `${saleYear}/${Number(saleMonth) < 10 ? `0${saleMonth}` : saleMonth}/${Number(saleDay) < 10 ? `0${saleDay}`:saleDay}` },
                PROD_CD: saleProdCd,
                QTY: saleQty,
                UNIT_PRICE: salePrice,
                REMARKS: saleRemarks
            })
        });

        if (!resp.ok) {
            alert("판매 생성 실패");
            return;
        }
        opener.location.reload();
        self.close();
    }

}