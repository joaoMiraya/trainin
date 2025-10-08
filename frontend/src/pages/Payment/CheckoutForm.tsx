import { CardElement, useElements, useStripe } from "@stripe/react-stripe-js";
import { useEffect, useState } from "react";

export const CheckoutForm = () => {
    const stripe = useStripe();
    const elements = useElements();
    const [clientSecret, setClientSecret] = useState('');

/*     useEffect(() => {
        // 1. Chama sua API para criar uma Payment Intent
        fetch('/api/create-payment-intent', {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({ amount: 1000 }) // exemplo: R$10,00 (centavos)
        })
          .then((res) => res.json())
          .then((data) => setClientSecret(data.clientSecret));
    }, []); */

    const handleSubmit = async (event: any) => {
        event.preventDefault();

        if (!stripe || !elements) return;

        const result = await stripe.confirmCardPayment(clientSecret, {
            payment_method: {
            card: elements.getElement(CardElement)!
            }
        });

        if (result.error) {
            console.error(result.error.message);
        } else {
            if (result.paymentIntent.status === 'succeeded') {
                alert('Pagamento realizado com sucesso!');
            }
        }
    };
    
    return (

        <>
            <form onSubmit={handleSubmit}>
                <CardElement />
                <button type="submit" disabled={!stripe}>Pagar</button>
            </form>
        </>
    );
}