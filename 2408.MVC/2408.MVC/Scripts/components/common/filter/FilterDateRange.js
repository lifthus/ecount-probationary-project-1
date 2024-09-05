export class FilterDateRange extends HTMLElement {
    constructor() {
        super();
    }
    connectedCallback() {
        const startDateId = this.getAttribute("startDateId");
        const endDateId = this.getAttribute("endDateId");
        const defaultStartDate = new Date(this.getAttribute("defaultStartDate"));
        const defaultStartYear = defaultStartDate.getFullYear() || "";
        let defaultStartMonth = (defaultStartDate.getMonth() + 1).toString() || "";
        if (defaultStartMonth.length === 1) {
            defaultStartMonth = '0' + defaultStartMonth;
        }
        let defaultStartDay = (defaultStartDate.getDate()).toString() || "";
        if (defaultStartDay.length === 1) {
            defaultStartDay = '0' + defaultStartDay;
        }
        let startDateValue = "";
        if (defaultStartDate.toString() !== "Invalid Date") startDateValue = `${defaultStartYear}/${defaultStartMonth}/${defaultStartDay}`;
        const defaultEndDate = new Date(this.getAttribute("defaultEndDate"));
        const defaultEndYear = defaultEndDate.getFullYear() || "";
        let defaultEndMonth = (defaultEndDate.getMonth() + 1).toString() || "";
        if (defaultEndMonth.length === 1) {
            defaultEndMonth = '0' + defaultEndMonth;
        }
        let defaultEndDay = (defaultEndDate.getDate()).toString() || "";
        if (defaultEndDay.length === 1) {
            defaultEndDay = '0' + defaultEndDay;
        }
        let endDateValue = "";
        if (defaultEndDate.toString() !== "Invalid Date") endDateValue = `${defaultEndYear}/${defaultEndMonth}/${defaultEndDay}`;
        const content = this.innerHTML.trim() || href;
        this.innerHTML = `
          <div class="flex bg-whitesmoke bd-solid bd-sm p-sm">
            <input
                type="hidden"
                id="${startDateId}"
                value="${startDateValue}"
            />
            <input
                type="hidden"
                id="${endDateId}"
                value="${endDateValue}"
            />
            <label
                class="w-100px"
            >
                ${content}
            </label>
            <select
                id="filter-start-year"
                name="filter-start-year"
            >
                ${Array.from({ length: 50 }, (_, i) => {
                    const thisYear = new Date().getFullYear() - i;
                    return `<option value="${thisYear}" ${thisYear === defaultStartYear ? "selected" : ""}>${new Date().getFullYear() - i}</option>`;
                }).join("")}
                <option value="1900" ${defaultStartYear === 1900 ? 'selected' : ''}>1900</option>
            </select>
            /
            <select
                id="filter-start-month"
                name="filter-start-month"
            >
                ${Array.from({ length: 12 }, (_, i) => {
                    const thisMonth = i + 1;
                    return `<option value="${i+1<10?'0':''}${i + 1}" ${thisMonth === Number(defaultStartMonth) ? "selected" : ""}>${i + 1}</option>`;
                }).join("")}
            </select>
            /
            <select
                id="filter-start-day"
                name="filter-start-day"
            >
                ${Array.from({ length: 31 }, (_, i) => {
                    const thisDay = i + 1;
                    return `<option value="${i + 1 < 10 ? '0' : ''}${i + 1}" ${thisDay === Number(defaultStartDay) ? "selected" : ""}>${i + 1}</option>`;
                }).join("")}
            </select>
            ~
            <select
                id="filter-end-year"
                name="filter-end-year"
            >
                ${Array.from({ length: 50 }, (_, i) => {
                    const thisYear = new Date().getFullYear() - i;
                    return `<option value=${new Date().getFullYear() - i} ${thisYear === defaultEndYear ? "selected" : ""}>${
                        new Date().getFullYear() - i
                    }</option>`;
                }).join("")}
                <option value="9999" ${defaultEndYear === 9999 ? 'selected' : ''}>9999</option>
            </select>
            /
            <select
                id="filter-end-month"
                name="filter-end-month"
            >
                ${Array.from({ length: 12 }, (_, i) => {
                    const thisMonth = i + 1;
                    return `<option value="${i + 1 < 10 ? '0' : ''}${i + 1}" ${thisMonth === Number(defaultEndMonth) ? "selected" : ""}>${i + 1}</option>`;
                }).join("")}
            </select>
            /
            <select
                id="filter-end-day"
                name="filter-end-day"
            >
                ${Array.from({ length: 31 }, (_, i) => {
                    const thisDay = i + 1;
                    return `<option value="${i + 1 < 10 ? '0' : ''}${i + 1}" ${thisDay === Number(defaultEndDay) ? "selected" : ""}>${i + 1}</option>`;
                }).join("")}
            </select>
        </div>
        `;
        const startDateInput = document.getElementById(startDateId);
        const endDateInput = document.getElementById(endDateId);
        const startYearInput = document.getElementById("filter-start-year");
        const startMonthInput = document.getElementById("filter-start-month");
        const startDayInput = document.getElementById("filter-start-day");
        const endYearInput = document.getElementById("filter-end-year");
        const endMonthInput = document.getElementById("filter-end-month");
        const endDayInput = document.getElementById("filter-end-day");
        startYearInput.addEventListener("change", () => {
            const startDate = `${startYearInput.value || "N"}/${startMonthInput.value || "N"}/${startDayInput.value || "N"}`;
            const isValidDate = new Date(startDate).toString() !== "Invalid Date";
            if (isValidDate) {
                startDateInput.value = startDate;
                console.log(startDate);
            } else {
                startDateInput.value = "";
            }
        });
        startMonthInput.addEventListener("change", () => {
            const startDate = `${startYearInput.value || "N"}/${startMonthInput.value || "N"}/${startDayInput.value || "N"}`;
            const isValidDate = new Date(startDate).toString() !== "Invalid Date";
            if (isValidDate) {
                startDateInput.value = startDate;
            } else {
                startDateInput.value = "";
            }
        });
        startDayInput.addEventListener("change", () => {
            const startDate = `${startYearInput.value || "N"}/${startMonthInput.value || "N"}/${startDayInput.value || "N"}`;
            const isValidDate = new Date(startDate).toString() !== "Invalid Date";
            if (isValidDate) {
                startDateInput.value = startDate;
            } else {
                startDateInput.value = "";
            }
        });
        endYearInput.addEventListener("change", () => {
            const endDate = `${endYearInput.value || "N"}/${endMonthInput.value || "N"}/${endDayInput.value || "N"}`;
            const isValidDate = new Date(endDate).toString() !== "Invalid Date";
            if (isValidDate) {
                endDateInput.value = endDate;
            } else {
                endDateInput.value = "";
            }
        });
        endMonthInput.addEventListener("change", () => {
            const endDate = `${endYearInput.value || "N"}/${endMonthInput.value || "N"}/${endDayInput.value || "N"}`;
            const isValidDate = new Date(endDate).toString() !== "Invalid Date";
            if (isValidDate) {
                endDateInput.value = endDate;
            } else {
                endDateInput.value = "";
            }
        });
        endDayInput.addEventListener("change", () => {
            const endDate = `${endYearInput.value || "N"}/${endMonthInput.value || "N"}/${endDayInput.value || "N"}`;
            const isValidDate = new Date(endDate).toString() !== "Invalid Date";
            if (isValidDate) {
                endDateInput.value = endDate;
            } else {
                endDateInput.value = "";
            }
        });
    }
}
