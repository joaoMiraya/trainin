export interface PaymentIntentRequest {
    providerUid: string;
    providerDomain: string;
    userIp: string;
    userEmail: string;
    userName: string;
    webAgent: string;
    amount: string;
    currency: CurrencyAvailable;
    description: string;
};

export type CurrencyAvailable = 'BRL' | 'USD';