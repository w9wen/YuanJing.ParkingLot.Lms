import { UserModel } from "./user-model";

export class EmployeeParams {
    gender: string;
    City: string;
    Country: string;
    pageNumber = 1;
    pageSize = 24;
    orderBy = "updatedDateTime";

    constructor(user: UserModel) {
        this.gender = user.gender === 'female' ? 'male' : 'female';
    }
}
