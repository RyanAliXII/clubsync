import { definePreset } from '@primeng/themes';
import Aura from '@primeng/themes/aura';

export const ClubSyncPreset = definePreset(Aura, {
    //Your customizations, see the following sections for examples
    semantic:{
        primary: {
            50: '{zinc.50}',
            100: '{zinc.100}',
            200: '{zinc.200}',
            300: '{zinc.300}',
            400: '{zinc.400}',
            500: '{zinc.500}',
            600: '{zinc.600}',
            700: '{zinc.700}',
            800: '{zinc.800}',
            900: '{zinc.900}',
            950: '{zinc.950}'
        }
    }
   
});

