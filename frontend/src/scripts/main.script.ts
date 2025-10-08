export const phoneFormater = (value: string) => {
  const cleaned = value.replace(/\D/g, '').slice(0, 11);
  const match = cleaned.match(/^(\d{0,2})(\d{0,5})(\d{0,4})$/);

  if (match) {
    const [, ddd, part1, part2] = match;
    if (part2) {
      return `(${ddd}) ${part1}-${part2}`;
    } else if (part1) {
      return `(${ddd}) ${part1}`;
    } else if (ddd) {
      return `(${ddd}`;
    }
  }

  return '';
};
