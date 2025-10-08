import {loadStripe} from '@stripe/stripe-js';
import {Elements} from '@stripe/react-stripe-js';
import { CheckoutForm } from './CheckoutForm';

const stripePromise = loadStripe(import.meta.env.VITE_STRIPE_KEY_TEST);

export const Payment = () => {
    const options = {
        // passing the client secret obtained from the server
        clientSecret: '{{CLIENT_SECRET}}',
      };
    return (
        <Elements stripe={stripePromise} /* options={options} */>
        <CheckoutForm />
      </Elements>
    )
}