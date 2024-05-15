export interface IServiceItem {
    id: number
    title: string
    children: IServiceItem[] | null
}